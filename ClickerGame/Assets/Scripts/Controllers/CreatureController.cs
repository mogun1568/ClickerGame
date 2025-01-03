using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    Define.State _state = Define.State.Idle;
    public Define.State State
    {
        get { return _state; }
        set
        {
            if (_state == value || _state == Define.State.Dead)
                return;

            _state = value;
            UpdateAnimation();
        }
    }

    StatInfo _stat = new StatInfo();
    public StatInfo Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    protected Animator _animator;
    protected string _targetTag;
    protected GameObject _target;
    private bool DeadFlag;

    private void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        _state = Define.State.Idle;
        _animator = GetComponent<Animator>();
        DeadFlag = false;
        UpdateAnimation();
    }

    private void UpdateTarget()
    {
        if (State == Define.State.Dead)
            return;

        GameObject[] targets = GameObject.FindGameObjectsWithTag(_targetTag);
        GameObject nearestTarget = null;

        foreach (GameObject target in targets)
        {
            if (target.GetComponent<CreatureController>().State == Define.State.Dead)
                continue;

            float disToTarget = Mathf.Abs(target.transform.position.x - transform.position.x);
            //Debug.Log(disToEnemy); 

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
        if (State == Define.State.Dead)
        {
            if (!DeadFlag)
            {
                DeadFlag = true;
                UpdateController();
            }

            return;
        }
        
        UpdateController();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Attacking:
                UpdateAttacking();
                break;
            case Define.State.Hurt:
                UpdateHurt();
                break;
            case Define.State.Dead:
                UpdateDead();
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
                //_animator.Play("Idle");
                _animator.SetBool("IsIdle", true);
                _animator.SetBool("IsAttack", false);
                break;
            case Define.State.Moving:
                //_animator.Play("Run");
                _animator.SetBool("IsIdle", false);
                break;
            case Define.State.Attacking:
                //_animator.Play("Attack");
                _animator.SetBool("IsAttack", true);
                break;
            case Define.State.Hurt:
                //_animator.Play("Hurt");
                _animator.SetTrigger("HurtTrigger");
                break;
            case Define.State.Dead:
                //_animator.Play("Die");
                _animator.SetTrigger("DeathTrigger");
                break;
        }
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

    protected virtual void UpdateDead()
    {
        
    }

    protected virtual void Hurt(float damage)
    {
        Stat.HP -= damage;
        //Debug.Log($"{gameObject.tag}, {Stat.HP}");

        if (Stat.HP <= 0)
            State = Define.State.Dead;
        else
            State = Define.State.Hurt;
    }
}
