using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
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
            //if (gameObject.tag == "Player") Debug.Log(_state);
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
            // �ӽ�
            if (gameObject.tag == "Player")
                return true;

            //Debug.Log($"{_curAnimInfo.IsName("Hurt")}, {_curAnimInfo.normalizedTime}");
            if (!CheckAnim())
                return false;
        }

        if (_state == Define.State.Attack)
        {
            // �ӽ�
            // �÷��̾�� ���� �ڵ尡 Attack �ʿ��� ��������� ���°� Hurt�� �ٲ��� �ʾƵ� �ȴ�.
            // ����� Attack�� ���۰� ���ÿ� ���� �ڵ尡 ���������
            // �����ӿ� ���� �����ϸ� Į�� �´� ������ ������ ���� �ڵ尡 ������� ���� ���̴�.
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
        if (_curAnimInfo.normalizedTime < 1.0f) // ������ �ƴ� ��쿡��
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
        get { return StatInfo.MaxHP; }
        set
        {
            float previousMaxHP = StatInfo.MaxHP;
            StatInfo.MaxHP = value;

            float increaseAmount = value - previousMaxHP;
            HP = Mathf.Min(HP + increaseAmount, StatInfo.MaxHP);
        }
    }

    public virtual float HP
    {
        get { return StatInfo.HP; }
        set { StatInfo.HP = value; }
    }

    public virtual float AttackSpeed
    {
        get { return StatInfo.AttackSpeed; }
        set
        {
            StatInfo.AttackSpeed = Mathf.Round(value * 100f) / 100f;
            _animator.SetFloat("AttackSpeed", StatInfo.AttackSpeed);
        }
    }

    protected virtual void UpdateStat()
    {

    }

    protected Animator _animator;
    protected AnimatorStateInfo _curAnimInfo;
    protected Coroutine _AttackCoroutine;

    protected string _targetTag;
    protected GameObject _target;
    private bool DeadFlag;

    [SerializeField]
    protected float _moveSpeed = 2.5f;

    private void OnEnable()
    {
        Init().Forget();
    }

    protected virtual async UniTask Init()
    {
        // �� MyPlayerController���� Run���� �����ϸ� ù �ִϰ� ������ �ȵǴ��� ���� �ǹ�
        if (gameObject.tag == "Player")
            _state = Define.State.Run;
        else
            _state = Define.State.Idle;

        _animator = GetComponent<Animator>();
        DeadFlag = false;
        UpdateAnimation();

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
    }

    protected virtual void Move(float endPosX, float moveSpeed)
    {

    }

    private void UpdateTarget()
    {
        if (DeadFlag)
            return;

        GameObject[] targets = GameObject.FindGameObjectsWithTag(_targetTag);
        GameObject nearestTarget = null;

        foreach (GameObject target in targets)
        {
            if (target.GetComponent<CreatureController>().State == Define.State.Death)
                continue;

            float disToTarget = Mathf.Abs(target.transform.position.x - transform.position.x);

            if (disToTarget <= StatInfo.Range)
            {
                nearestTarget = target;
                break;
            }
        }

        if (nearestTarget != null)
            _target = nearestTarget;
        else
            _target = null;
    }

    protected virtual void Update()
    {
        if (DeadFlag)
            return;

        // _animator�� Ư¡������ �ﰢ ������ �ݿ����� �ʾ� _curAnimInfo�� ���� �ִ��� �� ����
        // �� �κ��� �ذ��ؾ� �ִϰ� �Ų������� -> ���� ���� �ִϿ� �ð��� �� �� üũ�ϴ� ������� �ذ�
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
                // �ӽ�
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

    protected virtual void UpdateDie()
    {
        //_animator = null;
        _AttackCoroutine = null;

        _targetTag = null;
        _target = null;
        DeadFlag = true;

        StartCoroutine(DeadAnim(1.1f));
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
        }
    }

    protected virtual void Hurt(float damage)
    {
        StatInfo.HP -= damage;
        StatInfo.HP = Mathf.Max(StatInfo.HP, 0);
        //Debug.Log($"{gameObject.tag}, {StatInfo.HP}");

        if (StatInfo.HP <= 0)
            State = Define.State.Death;
        else
            State = Define.State.Hurt;
    }

    protected virtual IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        Managers.Resource.Destroy(gameObject);
    }
}
