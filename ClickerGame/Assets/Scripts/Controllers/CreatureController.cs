using UnityEngine;

public class StatInfo
{
    public double HP { get; set; }
    public double ATK { get; set; }
    public double DEF { get; set; }
    public double AttackSpeed { get; set; }
        
}

public class CreatureController : MonoBehaviour
{
    StatInfo _stat = new StatInfo();
    public StatInfo Stat
    {
        get { return _stat; }
        set { _stat = value; }
    }

    protected Animator _animator;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
    }
}
