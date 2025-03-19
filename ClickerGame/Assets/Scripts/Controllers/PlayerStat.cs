using System.Collections.Generic;

public class PlayerStat : Stat
{
    private Data.Stat _statMaxHP;
    private Data.Stat _statRegeneration;
    private Data.Stat _statATK;
    private Data.Stat _statDEF;
    private Data.Stat _statAttackSpeed;
    private Data.Stat _statRange;

    public PlayerStat(Dictionary<string, Data.Stat> statDict)
    {
        _statMaxHP = statDict["MaxHP"];
        _statRegeneration = statDict["Regeneration"];
        _statATK = statDict["ATK"];
        _statDEF = statDict["DEF"];
        _statAttackSpeed = statDict["AttackSpeed"];
        _statRange = statDict["Range"];
    }

    public override int Coin
    {
        get => Managers.Data.MyPlayerInfo.Coin;
        set
        {
            Managers.Data.MyPlayerInfo.Coin = value;
            Managers.Data.UpdateInfo("Coin", value);
        }
    }

    public override float HP
    {
        get => Managers.Data.MyPlayerInfo.HP;
        set
        {
            base.HP = value;
            Managers.Data.MyPlayerInfo.HP = base.HP;
            Managers.Data.UpdateInfo("HP", base.HP);
        }
    }

    public override float MaxHP
    {
        get => _statMaxHP.statValue;
        set
        {
            base.MaxHP = value;
            _statMaxHP.statValue = base.MaxHP;
            Managers.Data.UpdateDict("MaxHP");
        }
    }

    public float Regeneration
    {
        get => _statRegeneration.statValue;
        set
        {   
            _statRegeneration.statValue = value;
            Managers.Data.UpdateDict("Regeneration");
        }
    }

    public override float ATK
    {
        get => _statATK.statValue;
        set
        {
            base.ATK = value;
            _statATK.statValue = base.ATK;
            Managers.Data.UpdateDict("ATK");
        }
    }

    public override float DEF
    {
        get => _statDEF.statValue;
        set
        {
            base.DEF = value;
            _statDEF.statValue = base.DEF;
            Managers.Data.UpdateDict("DEF");
        }
    }

    public override float AttackSpeed
    {
        get => _statAttackSpeed.statValue;
        set
        {
            base.AttackSpeed = value;
            _statAttackSpeed.statValue = base.AttackSpeed;
            Managers.Data.UpdateDict("AttackSpeed");
        }
    }

    public override float Range
    {
        get => _statRange.statValue;
        set
        {
            base.Range = value;
            _statRange.statValue = base.Range;
            Managers.Data.UpdateDict("Range");
        }
    }
}
