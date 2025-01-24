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
        StatInfo.AttackCountdown = 0;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    protected override void UpdateStat()
    {
        StatInfo.MaxHP = _statDict["MaxHP"].statValue;
        StatInfo.IncreaseMaxHP = _statDict["MaxHP"].statIncreaseValue;
        StatInfo.HP = _statDict["HP"].statValue;

        StatInfo.ATK = _statDict["ATK"].statValue;
        StatInfo.IncreaseATK = _statDict["ATK"].statIncreaseValue;

        StatInfo.DEF = _statDict["DEF"].statValue;
        StatInfo.IncreaseDEF = _statDict["DEF"].statIncreaseValue;

        AttackSpeed = _statDict["AttackSpeed"].statValue;
        StatInfo.IncreaseAttackSpeed = _statDict["AttackSpeed"].statIncreaseValue;

        StatInfo.Range = _statDict["Range"].statValue;
        StatInfo.IncreaseRange = _statDict["Range"].statIncreaseValue;
    }

    public void UpdateDict()
    {
        _statDict["MaxHP"].statValue = StatInfo.MaxHP;
        _statDict["HP"].statValue = StatInfo.HP;
        _statDict["ATK"].statValue = StatInfo.ATK;
        _statDict["DEF"].statValue = StatInfo.DEF;
        _statDict["AttackSpeed"].statValue = AttackSpeed;
        _statDict["Range"].statValue = StatInfo.Range;

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
