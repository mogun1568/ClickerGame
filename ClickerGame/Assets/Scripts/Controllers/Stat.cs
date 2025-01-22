using System.Collections.Generic;

public class Stat
{
    public float HP { get; set; }
    public float IncreaseHP { get; set; }

    public float ATK { get; set; }
    public float IncreaseATK { get; set; }

    public float DEF { get; set; }
    public float IncreaseDEF { get; set; }

    public float AttackSpeed { get; set; }
    public float IncreaseAttackSpeed { get; set; }

    public float AttackCountdown { get; set; }

    public float Range { get; set; }
    public float IncreaseRange { get; set; }

    public void UpdateStat(Dictionary<string, Data.Stat> _statDict)
    {
        HP = _statDict["HP"].statValue;
        IncreaseHP = _statDict["HP"].statIncreaseValue;

        ATK = _statDict["ATK"].statValue;
        IncreaseATK = _statDict["ATK"].statIncreaseValue;

        DEF = _statDict["DEF"].statValue;
        IncreaseDEF = _statDict["DEF"].statIncreaseValue;

        AttackSpeed = _statDict["AttackSpeed"].statValue;
        IncreaseAttackSpeed = _statDict["AttackSpeed"].statIncreaseValue;

        Range = _statDict["Range"].statValue;
        IncreaseRange = _statDict["Range"].statIncreaseValue;
    }

    public void UpdateEnemy(Data.Enemy enemyInfo)
    {
        HP = enemyInfo.enemyHP;
        ATK = enemyInfo.enemyATK;
        DEF = enemyInfo.enemyDEF;
        AttackSpeed = enemyInfo.enemyAttackSpeed;
        Range = enemyInfo.enemyRange;
    }


    /*
    private float _hp;
    private float _atk;
    private float _def;
    private float _attackSpeed;
    private float _attackCountdown;
    private float _range;

    public float HP
    {
        get { return _hp; }
        set { _hp = value; }
    }

    public float ATK
    {
        get { return _atk; }
        set { _atk = value; }
    }

    public float DEF
    {
        get { return _def; }
        set { _attackSpeed = value; }
    }

    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    public float AttackCountdown
    {
        get { return _attackCountdown; }
        set { _attackCountdown = value; }
    }

    public float Range
    {
        get { return _range; }
        set { _range = value; }
    }
    */
}
