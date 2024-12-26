using UnityEngine;

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
    Define.State _state = Define.State.Run;
    public Define.State State
    {
        get { return _state; }
        set { _state = value; }
    }

    StatInfo _stat = new StatInfo();
    public StatInfo Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    protected Animator _animator;

    void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Attack(GameObject go, float damage)
    {
        //Debug.Log("Attack!");
        go.GetComponent<CreatureController>().TakeDamage(damage);
    }

    protected virtual void TakeDamage(float damage)
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
