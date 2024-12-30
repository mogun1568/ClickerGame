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
    Define.State _state = Define.State.Moving;
    public Define.State State
    {
        get { return _state; }
        set { _state = value; UpdateAnimation(); }
    }

    StatInfo _stat = new StatInfo();
    public StatInfo Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    protected Animator _animator;

    private void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        UpdateAnimation();
    }

    protected virtual void Update()
    {
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
                UpdateAttacking();
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
                _animator.Play("Idle");
                break;
            case Define.State.Moving:
                _animator.Play("Run");
                break;
            case Define.State.Attacking:
                _animator.Play("Attack");
                break;
            case Define.State.Hurt:
                _animator.Play("Hurt");
                break;
            case Define.State.Dead:
                _animator.Play("Die");
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

    }

    protected virtual void UpdateDamaged()
    {

    }

    protected virtual void UpdateDead()
    {
        
    }

    protected virtual void Attack(GameObject go, float damage)
    {
        //Debug.Log("Attack!");
        go.GetComponent<CreatureController>().Hurt(damage);
    }

    protected virtual void Hurt(float damage)
    {
        Stat.HP -= damage;
        Debug.Log(Stat.HP);

        if (Stat.HP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        
    }
}
