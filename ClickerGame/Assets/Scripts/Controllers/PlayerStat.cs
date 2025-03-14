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
        get => Managers.Data.MyPlayerInfo.coin;
        set
        {
            Managers.Data.MyPlayerInfo.coin = value;
            Managers.Data.UpdateInfo("Coin", value);
        }
    }

    public override float HP
    {
        get => Managers.Data.MyPlayerInfo.HP;
        set
        {
            Managers.Data.MyPlayerInfo.HP = value;
            Managers.Data.UpdateInfo("HP", value);
        }
    }

    public override float MaxHP
    {
        get => _statMaxHP.statValue;
        set
        {
            _statMaxHP.statValue = value;
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
            _statATK.statValue = value;
            Managers.Data.UpdateDict("ATK");
        }
    }

    public override float DEF
    {
        get => _statDEF.statValue;
        set
        {
            _statDEF.statValue = value;
            Managers.Data.UpdateDict("DEF");
        }
    }

    public override float AttackSpeed
    {
        get => _statAttackSpeed.statValue;
        set
        {
            _statAttackSpeed.statValue = value;
            Managers.Data.UpdateDict("AttackSpeed");
        }
    }

    public override float Range
    {
        get => _statRange.statValue;
        set
        {
            _statRange.statValue = value;
            Managers.Data.UpdateDict("Range");
        }
    }

}
