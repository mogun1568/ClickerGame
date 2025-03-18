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
        set => _hp = Mathf.Clamp(Mathf.Round(value * 100f) / 100f, 0, MaxHP);
    }

    public virtual float ATK
    {
        get => _atk;
        set => _atk = Mathf.Round(value * 100f) / 100f;
    }

    public virtual float DEF
    {
        get => _def;
        set => _def = Mathf.Round(value * 100f) / 100f;
    }

    public virtual float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = Mathf.Round(value * 100f) / 100f;
    }

    public virtual float Range
    {
        get => _range;
        set => _range = Mathf.Round(value * 100f) / 100f;
    }

    public virtual float AttackCountdown
    {
        get => _attackCountdown;
        set => _attackCountdown = value;
    }

}
