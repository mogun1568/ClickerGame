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
        StatInfo.Coin = (int)_statDict["Coin"].statValue;

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

    public override void UpdateDict()
    {
        base.UpdateDict();

        _statDict["Coin"].statValue = StatInfo.Coin;
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

    // �÷��̾��� Stat�� �°� ���� Stat�� ������ �ڵ� ����
    // �ٸ� ��ũ���� �̵��� ����
    private void UpdateEnemyDict()
    {

        Data.EnemyData enemyData = new Data.EnemyData
        {
            enemys = new List<Data.Enemy>(Managers.Data.EnemyDict.Values)
        };

        Managers.Data.SaveJson(enemyData, "EnemyDataTest");
    }

    protected override void UpdateAttacking()
    {
        base.UpdateHurt();
        _AttackCoroutine = StartCoroutine(CheckAnimationTime(0.167f, StatInfo.ATK));
    }
}
