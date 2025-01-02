using UnityEngine;

public class MyPlayerController : CreatureController
{
    protected override void Init()
    {
        base.Init();

        Managers.Game.MyPlayer = this;

        Stat.HP = 100;
        Stat.ATK = 10;
        Stat.DEF = 50;
        Stat.Range = 1.5f;
        Stat.AttackSpeed = 1;
        Stat.AttackCountDown = 0;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    protected override void Update()
    {
        if (_target == null)
        {
            State = Define.State.Moving;
            return;
        }

        State = Define.State.Idle;

        Debug.Log(1);
        if (Stat.AttackCountDown <= 0)
        {
            State = Define.State.Attacking;
            Stat.AttackCountDown = 1 / Stat.AttackSpeed;
        }

        Stat.AttackCountDown -= Time.deltaTime;

        base.Update();
    }
}
