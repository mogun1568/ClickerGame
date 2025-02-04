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

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        _statDict = Managers.Data.MyPlayerStatDict;
        UpdateStat();
        StatInfo.AttackCountdown = 0;

        _targetTag = "Enemy";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    protected override void TargetIsNull()
    {
        base.TargetIsNull();

        if (StatInfo.AttackCountdown != 0)
            StatInfo.AttackCountdown = 0;
        State = Define.State.Run;
    }

    protected override void UpdateStat()
    {
        StatInfo.Coin = (int)_statDict["Coin"].statValue;
        MaxHP = _statDict["MaxHP"].statValue;
        HP = _statDict["HP"].statValue;
        StatInfo.ATK = _statDict["ATK"].statValue;
        StatInfo.DEF = _statDict["DEF"].statValue;
        AttackSpeed = _statDict["AttackSpeed"].statValue;
        StatInfo.Range = _statDict["Range"].statValue;
    }

    public override void UpdateDict()
    {
        base.UpdateDict();

        _statDict["Coin"].statValue = StatInfo.Coin;
        _statDict["MaxHP"].statValue = MaxHP;
        _statDict["HP"].statValue = HP;
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
        // ���� ü���̳� ���ݷ��� �÷��̾��� ���ȿ� ���� ��ȭ�ϰ� �� ��
        // �ƴϸ� �ð�? ����? � ���� �ٸ��� �� �� ��� ��

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
