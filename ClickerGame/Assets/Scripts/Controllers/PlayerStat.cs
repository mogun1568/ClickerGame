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
        set => Managers.Data.MyPlayerInfo.Coin = value;
    }

    public override float HP
    {
        get => Managers.Data.MyPlayerInfo.HP;
        set
        {
            base.HP = value;
            Managers.Data.MyPlayerInfo.HP = base.HP;
        }
    }

    public override float MaxHP
    {
        get => _statMaxHP.statValue;
        set
        {
            base.MaxHP = value;
            SetValue(ref _statMaxHP.statValue, value, "MaxHP");
        }
    }

    public float Regeneration
    {
        get => _statRegeneration.statValue;
        set => SetValue(ref _statRegeneration.statValue, value, "Regeneration");
    }

    public override float ATK
    {
        get => _statATK.statValue;
        set
        {
            base.ATK = value;
            SetValue(ref _statATK.statValue, value, "ATK");
        }
    }

    public override float DEF
    {
        get => _statDEF.statValue;
        set
        {
            base.DEF = value;
            SetValue(ref _statDEF.statValue, value, "DEF");
        }
    }

    public override float AttackSpeed
    {
        get => _statAttackSpeed.statValue;
        set
        {
            base.AttackSpeed = value;
            SetValue(ref _statAttackSpeed.statValue, value, "AttackSpeed");
        }
    }

    public override float Range
    {
        get => _statRange.statValue;
        set
        {
            base.Range = value;
            SetValue(ref _statRange.statValue, value, "Range");
        }
    }

    private void SetValue<T>(ref T field, T newValue, string key)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            Managers.Data.UpdateDict(key);
        }
    }
}
