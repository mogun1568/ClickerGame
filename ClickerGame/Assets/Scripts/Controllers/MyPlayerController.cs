using UnityEngine;

public class MyPlayerController : CreatureController
{
    protected override void Init()
    {
        base.Init();

        transform.position = new Vector3(-2, 1.9f, -1);

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        Stat.UpdateStat(Managers.Data.MyPlayerStatDict);
        Stat.AttackCountdown = 0;
        Stat.Range = 1.5f;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }
}
