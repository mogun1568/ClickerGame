using Data;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MyPlayerController : CreatureController
{
    private Dictionary<string, Data.Stat> _statDict;

    protected override void Init()
    {
        base.Init();

        transform.position = new Vector3(-2, 1.9f, -1);

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        _statDict = Managers.Data.MyPlayerStatDict;
        UpdateStat();
        Stat.AttackCountdown = 0;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    protected override void UpdateStat()
    {
        Stat.HP = _statDict["HP"].statValue;
        Stat.IncreaseHP = _statDict["HP"].statIncreaseValue;

        Stat.ATK = _statDict["ATK"].statValue;
        Stat.IncreaseATK = _statDict["ATK"].statIncreaseValue;

        Stat.DEF = _statDict["DEF"].statValue;
        Stat.IncreaseDEF = _statDict["DEF"].statIncreaseValue;

        Stat.AttackSpeed = _statDict["AttackSpeed"].statValue;
        Stat.IncreaseAttackSpeed = _statDict["AttackSpeed"].statIncreaseValue;

        Stat.Range = _statDict["Range"].statValue;
        Stat.IncreaseRange = _statDict["Range"].statIncreaseValue;
    }

    public void UpdateDict()
    {
        _statDict["HP"].statValue = Stat.HP;
        _statDict["ATK"].statValue = Stat.ATK;
        _statDict["DEF"].statValue = Stat.DEF;
        _statDict["AttackSpeed"].statValue = Stat.AttackSpeed;
        _statDict["Range"].statValue = Stat.Range;

        Data.StatData statData = new Data.StatData
        {
            stats = new List<Data.Stat>(Managers.Data.MyPlayerStatDict.Values)
        };

        Managers.Data.SaveJson(statData, "MyPlayerStatDataTest");
    }

    // 플레이어의 Stat에 맞게 적들 Stat도 변경할 코드 예정
    // 다른 스크립로 이동할 수도
    private void UpdateEnemyDict()
    {

        Data.EnemyData enemyData = new Data.EnemyData
        {
            enemys = new List<Data.Enemy>(Managers.Data.EnemyDict.Values)
        };

        Managers.Data.SaveJson(enemyData, "EnemyDataTest");
    }

    protected override void UpdateHurt()
    {
        base.UpdateHurt();

        UpdateDict();
    }
}
