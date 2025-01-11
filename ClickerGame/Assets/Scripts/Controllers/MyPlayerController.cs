using UnityEngine;

public class MyPlayerController : CreatureController
{
    protected override void Init()
    {
        base.Init();

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        Stat.HP = 100;
        Stat.ATK = 10;
        Stat.DEF = 50;
        Stat.Range = 1.5f;
        Stat.AttackSpeed = 1;
        Stat.AttackCountDown = 0;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }
}
