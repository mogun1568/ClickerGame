using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    Define.State _state;
    public Define.State State
    {
        get { return _state; }
        set
        {
            if (DeadFlag)
                return;

            if (!CheckState(value))
                return;

            if (_AttackCoroutine != null)
                StopCoroutine(_AttackCoroutine);

            _state = value;
            //if (gameObject.tag != "Player")
            //    Debug.Log($"{_state}");

            UpdateAnimation();
            UpdateController();
        }
    }

    private bool CheckState(Define.State state)
    {
        if (state == Define.State.Death)
            return true;

        //if (gameObject.tag != "Player")
        //    Debug.Log($"{_state} || {state}");

        if (state == _state)
        {
            if (state == Define.State.Idle || state == Define.State.Run)
                return false;

            return true;
        }

        if (state == Define.State.Stagger)
        {
            // 랜덤으로 경직 저항
            if (Random.value < StaggerResistance)
                return false;

            return true;
        }

        if (_state == Define.State.Attack || _state == Define.State.Stagger)
        {
            //Debug.Log($"{_curAnimInfo.IsName("Stagger")}, {_curAnimInfo.normalizedTime}");
            if (!CheckAnim())
                return false;
        }

        return true;
    }

    private bool CheckAnim()
    {
        //Debug.Log($"{gameObject.tag}, {_curAnimInfo.normalizedTime}");
        if (!_curAnimInfo.IsName(_state.ToString()))
            return false;
        if (_curAnimInfo.normalizedTime < 1.0f) // 루프가 아닌 경우에만
            return false;

        return true;
    }

    private Stat _stat = new Stat();
    public Stat StatInfo
    {
        get { return _stat; }
        set { _stat = value; }
    }

    public virtual float MaxHP
    {
        get => StatInfo.MaxHP;
        set => StatInfo.MaxHP = value;
    }

    public virtual float HP
    {
        get => StatInfo.HP;
        set => StatInfo.HP = value;
    }

    public virtual float AttackSpeed
    {
        get => StatInfo.AttackSpeed;
        set
        {
            StatInfo.AttackSpeed = value;
            _animator.SetFloat("AttackSpeed", StatInfo.AttackSpeed);
        }
    }

    public virtual float StaggerResistance
    {
        get => StatInfo.StaggerResistance;
        set => StatInfo.StaggerResistance = value;
    }

    protected Skill SkillInfo;

    protected Animator _animator;
    protected AnimatorStateInfo _curAnimInfo;
    protected Coroutine _AttackCoroutine;

    protected string _targetTag;
    protected GameObject _target;
    private bool DeadFlag;

    // 디버프 종류가 늘어나면 클래스 같은걸로 관리할 수도
    [HideInInspector] public Define.Debuff _debuff;
    [HideInInspector] public Define.TweenType _tweenType;

    [HideInInspector] public Tween MoveTween;
    [HideInInspector] public float _endPosX;
    [HideInInspector] public float _backgroundMoveSpeed;
    [HideInInspector] public float _moveSpeed;
    [HideInInspector] public float _curMoveSpeed;
    private bool _isUpdateTargetRunning;

    private void OnEnable()
    {
        InitAsync().Forget();
    }

    protected virtual async UniTask InitAsync()
    {
        _state = Define.State.None;

        _animator = GetComponent<Animator>();
        DeadFlag = false;
        UpdateAnimation();

        _debuff = Define.Debuff.None;
        _tweenType = Define.TweenType.Idle;
        MoveTween.Kill();
        _backgroundMoveSpeed = 2.5f;
        _curMoveSpeed = 0;
        _isUpdateTargetRunning = false;

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
    }

    private int GetPriority(Define.TweenType type)
    {
        return type switch
        {
            //Define.TweenType.Idle => 0,
            Define.TweenType.Run => 1,
            Define.TweenType.Slow => 2,
            Define.TweenType.Knockback => 3,
            _ => 0
        };
    }

    public virtual void Move(float endPosX, float moveSpeed, Define.TweenType tweenType)
    {
        if (State == Define.State.Death)
            return;

        if (GetPriority(tweenType) < GetPriority(_tweenType))
            return; // 더 낮은 우선순위면 무시

        if (moveSpeed == _curMoveSpeed)
            return;

        _curMoveSpeed = moveSpeed;
        _tweenType = tweenType;

        //if (gameObject.tag != "Player")
        //    Debug.Log($"{moveSpeed}, {tweenType}");

        if (MoveTween != null)
            MoveTween.Kill();

        float duration = Mathf.Abs(transform.position.x - endPosX) / moveSpeed;

        Tween newTween = null;
        newTween = transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear)
            .SetAutoKill(true)
            .OnKill(() =>
            {
                if (MoveTween == newTween)
                    MoveTween = null;

                // Knockback 끝났으면 Slow 재적용
                if (tweenType == Define.TweenType.Knockback)
                {
                    if (_debuff == Define.Debuff.Slow)
                        _tweenType = Define.TweenType.Slow;
                    else
                        _tweenType = Define.TweenType.Run;
                }
            })
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                TweenComplete();
            });

        MoveTween = newTween;
    }

    protected virtual void TweenComplete()
    {
        if (!_isUpdateTargetRunning)
        {
            _isUpdateTargetRunning = true;
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
        }
    }

    protected void UpdateTarget()
    {
        if (DeadFlag)
            return;

        // _targetTag가 null일 수가 있나?
        if (string.IsNullOrEmpty(_targetTag))
        {
            _target = null;
            return;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(_targetTag);

        if (targets.Length == 0)
        {
            _target = null;
            return;
        }

        GameObject nearestTarget = null;

        foreach (GameObject target in targets)
        {
            if (target.GetComponent<CreatureController>().State == Define.State.None ||
                target.GetComponent<CreatureController>().State == Define.State.Death)
                continue;

            float disToTarget = Mathf.Abs(target.transform.position.x - transform.position.x);

            if (disToTarget <= StatInfo.Range)
            {
                nearestTarget = target;
                break;
            }
        }

        _target = nearestTarget;
    }

    protected virtual void Update()
    {
        if (DeadFlag)
            return;

        // _animator의 특징때문에 즉각 적으로 반영되지 않아 _curAnimInfo가 이전 애니일 수 있음
        // 이 부분을 해결해야 애니가 매끄러워짐 -> 실행 중인 애니와 시간을 둘 다 체크하는 방식으로 해결
        _curAnimInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 목적지에 도착 전에 피격 받는 경우
        if (MoveTween != null)
        {
            if (State == Define.State.Stagger)
            {
                if (_tweenType == Define.TweenType.Run || _tweenType == Define.TweenType.Slow)
                {
                    if (MoveTween.IsPlaying())
                        MoveTween.Pause();
                }
            }
            else
            {
                if (!MoveTween.IsPlaying())
                    MoveTween.Play();
            }
        }

        if (_target == null)
        {
            TargetIsNull();
            return;
        }

        State = Define.State.Idle;

        if (StatInfo.AttackCountdown <= 0)
        {
            State = Define.State.Attack;
            StatInfo.AttackCountdown = 1 / StatInfo.AttackSpeed;
        }
        else
            StatInfo.AttackCountdown -= Time.deltaTime;
    }

    protected virtual void TargetIsNull()
    {
        if (State == Define.State.Death)
            return;
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Run:
                UpdateMoving();
                break;
            case Define.State.Attack:
                UpdateAttacking();
                break;
            case Define.State.Stagger:
                UpdateStagger();
                break;
            case Define.State.Death:
                UpdateDie();
                break;
        }
    }

    protected virtual void UpdateAnimation()
    {
        if (_animator == null)
            return;

        switch (State)
        {
            case Define.State.Idle:
                _animator.Play("Idle");
                //_animator.SetBool("IsIdle", true);
                //_animator.SetBool("IsAttack", false);
                break;
            case Define.State.Run:
                _animator.Play("Run");
                //_animator.SetBool("IsIdle", false);
                break;
            case Define.State.Attack:
                _animator.Play("Attack");
                //_animator.SetBool("IsAttack", true);
                break;
            case Define.State.Stagger:
                // 임시
                //if (gameObject.tag == "Player")
                //    return;

                _animator.Play("Stagger");
                //_animator.SetTrigger("HurtTrigger");
                break;
            case Define.State.Death:
                _animator.Play("Death");
                //_animator.SetTrigger("DeathTrigger");
                break;
        }

        //_curAnimInfo = _animator.GetCurrentAnimatorStateInfo(0);
    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateMoving()
    {
        
    }

    protected virtual void UpdateAttacking()
    {
        //Debug.Log("Attack!");
    }

    protected virtual void UpdateStagger()
    {

    }

    protected virtual void UpdateDie()
    {
        _AttackCoroutine = null;
        CancelInvoke(nameof(UpdateTarget));

        _targetTag = null;
        _target = null;
        DeadFlag = true;

        StartCoroutine(DeadAnim(1f));
    }

    protected virtual IEnumerator CheckAnimationTime(float targetNormalizedTime, float amount)
    {
        yield return new WaitUntil(() =>
        {
            return _curAnimInfo.IsName(_state.ToString()) && _curAnimInfo.normalizedTime % 1 >= targetNormalizedTime;
        });

        if (_target != null)
        {
            //Debug.Log($"{gameObject.tag}, {_curAnimInfo.normalizedTime % 1}");
            _target.GetComponent<CreatureController>().Hurt(amount);

            Skill();
        }
    }

    protected virtual void Skill() { 
    }

    protected virtual float CalculateDamage(float damage)
    {
        float reducedDamage = 100f / (100f + StatInfo.DEF);
        return damage * reducedDamage;
    }

    protected virtual void Hurt(float damage)
    {
        HP -= CalculateDamage(damage);
        //Debug.Log($"{gameObject.tag}, {HP}");

        if (HP <= 0)
            State = Define.State.Death;
        else
            State = Define.State.Stagger;
    }

    protected virtual IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Destroy(gameObject);
    }
}
