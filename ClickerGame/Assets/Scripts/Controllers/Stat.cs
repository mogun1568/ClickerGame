using UnityEngine;

public class Stat
{
    private int _coin;
    private float _maxHP;
    private float _hp;
    private float _atk;
    private float _def;
    private float _attackSpeed;
    private float _range;
    private float _staggerResistance;   // 경직 저항
    private float _attackCountdown = 0f;

    public virtual int Coin
    {
        get => _coin;
        set => _coin = value;
    }

    public virtual float MaxHP
    {
        get => _maxHP;
        set
        {
            float increaseAmount = value - _maxHP;
            _maxHP = value;
            HP += increaseAmount;
        }
    }

    public virtual float HP
    {
        get => _hp;
        set => _hp = ClampAndRound(value, 0, MaxHP);
    }

    public virtual float ATK
    {
        get => _atk;
        set => _atk = RoundToTwoDecimals(value);
    }

    public virtual float DEF
    {
        get => _def;
        set => _def = RoundToTwoDecimals(value);
    }

    public virtual float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = RoundToTwoDecimals(value);
    }

    public virtual float Range
    {
        get => _range;
        set => _range = RoundToTwoDecimals(value);
    }

    public virtual float StaggerResistance
    {
        get => _staggerResistance;
        set => _staggerResistance = RoundToTwoDecimals(value);
    }

    public virtual float AttackCountdown
    {
        get => _attackCountdown;
        set => _attackCountdown = value;
    }

    private float RoundToTwoDecimals(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }

    private float ClampAndRound(float value, float min, float max)
    {
        return Mathf.Clamp(RoundToTwoDecimals(value), min, max);
    }
}
