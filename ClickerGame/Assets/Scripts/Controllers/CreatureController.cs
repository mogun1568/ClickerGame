using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class StatInfo
{
    public float HP { get; set; }
    public float ATK { get; set; }
    public float DEF { get; set; }
    public float Range { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackCountDown { get; set; }
}

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
            if (gameObject.tag != "Player" || state == Define.State.Hurt)
                return true;

            if (!CheckAnim())
                return false;
        }

        return true;
    }

    private bool CheckAnim()
    {
        if (!_curAnimInfo.IsName(_state.ToString()))
            return false;
        if (_curAnimInfo.normalizedTime < 1.0f) // 루프가 아닌 경우에만
            return false;

        return true;
    }

    StatInfo _stat = new StatInfo();
    public StatInfo Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    protected Animator _animator;
    AnimatorStateInfo _curAnimInfo;

    protected string _targetTag;
    protected GameObject _target;
    private bool DeadFlag;

    private void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        // 왜 MyPlayerController에서 Run으로 설정하면 첫 애니가 적용이 안되는지 아직 의문
        if (gameObject.tag == "Player")
            _state = Define.State.Run;
        else
            _state = Define.State.Idle;

        _animator = GetComponent<Animator>();
        DeadFlag = false;
        UpdateAnimation();
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

            if (disToTarget <= Stat.Range)
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

        // _animator의 특징때문에 즉각 적으로 반영되지 않아 _curAnimInfo가 이전 애니일 수 있음
        // 이 부분을 해결해야 애니가 매끄러워짐 -> 실행 중인 애니와 시간을 둘 다 체크하는 방식으로 해결
        _curAnimInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_target == null)
        {
            if (gameObject.tag == "Player")
            {
                if (Stat.AttackCountDown != 0) 
                    Stat.AttackCountDown = 0;
                State = Define.State.Run;
            }
            else
                State = Define.State.Idle;

            return;
        }

        State = Define.State.Idle;

        if (Stat.AttackCountDown <= 0)
        {
            State = Define.State.Attack;
            Stat.AttackCountDown = 1 / Stat.AttackSpeed;
        }

        Stat.AttackCountDown -= Time.deltaTime;

        //UpdateController();
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
        _target.GetComponent<CreatureController>().Hurt(Stat.ATK);
    }

    protected virtual void UpdateHurt()
    {

    }

    protected virtual void UpdateDie()
    {
        DeadFlag = true;
    }

    protected virtual void Hurt(float damage)
    {
        Stat.HP -= damage;
        //Debug.Log($"{gameObject.tag}, {Stat.HP}");

        if (Stat.HP <= 0)
            State = Define.State.Death;
        else
            State = Define.State.Hurt;
    }
}
