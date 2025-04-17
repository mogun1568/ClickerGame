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
            //if (gameObject.tag != "Player") Debug.Log(_state);

            UpdateAnimation();
            UpdateController();
        }
    }

    private bool CheckState(Define.State state)
    {
        if (state == Define.State.Death)
            return true;

        if (state == _state)
        {
            if (state == Define.State.Idle || state == Define.State.Run)
                return false;
            else
                return true;
        }

        if (_state == Define.State.Hurt)
        {
            // 임시
            if (gameObject.tag == "Player")
                return true;

            //Debug.Log($"{_curAnimInfo.IsName("Hurt")}, {_curAnimInfo.normalizedTime}");
            if (!CheckAnim())
                return false;
        }

        if (_state == Define.State.Attack)
        {
            // 임시
            // 플레이어는 피해 코드가 Attack 쪽에서 실행됨으로 상태가 Hurt로 바뀌지 않아도 된다.
            // 현재는 Attack의 시작과 동시에 피해 코드가 실행되지만
            // 프레임에 맞춰 수정하면 칼에 맞는 프레임 전에는 피해 코드가 실행되지 않을 것이다.
            if (gameObject.tag != "Player" && state == Define.State.Hurt)
                return true;

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

    protected Skill SkillInfo;

    protected Animator _animator;
    protected AnimatorStateInfo _curAnimInfo;
    protected Coroutine _AttackCoroutine;

    protected string _targetTag;
    protected GameObject _target;
    private bool DeadFlag;

    public Tween MoveTween;
    public float _endPosX;
    public float _moveSpeed;
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

        SkillInfo = GetComponent<Skill>();

        MoveTween.Kill();
        _moveSpeed = 2.5f;
        _isUpdateTargetRunning = false;

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
    }

    public virtual void Move(float endPosX, float moveSpeed)
    {
        if (State == Define.State.Death)
            return;

        if (MoveTween != null)
            MoveTween.Kill();

        float duration = Mathf.Abs(transform.position.x - endPosX) / moveSpeed;

        MoveTween = transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear)
            .SetAutoKill(true)
            .OnKill(() =>
            {
                MoveTween = null;
                if (transform.position.x != _endPosX)
                    Move(_endPosX, _moveSpeed);
            })
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                TweenComplete();
            });
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

        //if (nearestTarget != null)
        //    _target = nearestTarget;
        //else
        //    _target = null;
    }

    protected virtual void Update()
    {
        if (DeadFlag)
            return;

        // _animator의 특징때문에 즉각 적으로 반영되지 않아 _curAnimInfo가 이전 애니일 수 있음
        // 이 부분을 해결해야 애니가 매끄러워짐 -> 실행 중인 애니와 시간을 둘 다 체크하는 방식으로 해결
        _curAnimInfo = _animator.GetCurrentAnimatorStateInfo(0);

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

        StatInfo.AttackCountdown -= Time.deltaTime;

        //UpdateController();
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
            case Define.State.Hurt:
                UpdateHurt();
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
            case Define.State.Hurt:
                // 임시
                if (gameObject.tag == "Player")
                    return;

                _animator.Play("Hurt");
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

    protected virtual void UpdateHurt()
    {

    }

        //_animator = null;
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

            // 이 부분에서 스킬을 써야 할 듯
            // 스킬들 중 1렙 이상인 것들중에 랜덤으로 하나 쓰도록 해야 함
            // 뭔가 스킬이 늦게 써지는 느낌이 있음
            //if (gameObject.tag == "Player") SkillInfo.Knockback(_target);
        }
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
            State = Define.State.Hurt;
    }

    protected virtual IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Destroy(gameObject);
    }
}
