using System.Collections.Generic;

public class PlayerStat : Stat
{
    private Data.StatInfo _statMaxHP;
    private Data.StatInfo _statRegeneration;
    private Data.StatInfo _statATK;
    private Data.StatInfo _statDEF;
    private Data.StatInfo _statAttackSpeed;
    private Data.StatInfo _statRange;

    public PlayerStat(Dictionary<string, Data.StatInfo> statDict)
    {
        _statMaxHP = statDict["MaxHP"];
        _statRegeneration = statDict["Regeneration"];
        _statATK = statDict["ATK"];
        _statDEF = statDict["DEF"];
        _statAttackSpeed = statDict["AttackSpeed"];
        _statRange = statDict["Range"];

        StaggerResistance = 1f;
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
            SetValue(ref _statMaxHP.statValue, base.MaxHP, "MaxHP");
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
            SetValue(ref _statATK.statValue, base.ATK, "ATK");
        }
    }

    public override float DEF
    {
        get => _statDEF.statValue;
        set
        {
            base.DEF = value;
            SetValue(ref _statDEF.statValue, base.DEF, "DEF");
        }
    }

    public override float AttackSpeed
    {
        get => _statAttackSpeed.statValue;
        set
        {
            base.AttackSpeed = value;
            SetValue(ref _statAttackSpeed.statValue, base.AttackSpeed, "AttackSpeed");
        }
    }

    public override float Range
    {
        get => _statRange.statValue;
        set
        {
            base.Range = value;
            SetValue(ref _statRange.statValue, base.Range, "Range");
        }
    }

    private void SetValue<T>(ref T field, T newValue, string key)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;

            if (Managers.Data.GameDataReady)
                Managers.Data.UpdateStat(key);
        }
    }
}
