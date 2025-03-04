using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class MyPlayerController : CreatureController
{
    private Dictionary<string, Data.Stat> _statDict;
    public bool isMove;

    public float Regeneration
    {
        get { return _statDict["Regeneration"].statValue; }
        set { _statDict["Regeneration"].statValue = value; }
    }

    protected override async UniTask Init()
    {
        await base.Init();

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        _statDict = Managers.Data.MyPlayerStatDict;
        UpdateStat();

        _targetTag = "Enemy";

        if (transform.position.x == -2)
        {
            isMove = false;
            InvokeRepeating("UpdateTarget", 0f, 0.1f);
        }
        else
        {
            isMove = true;
            HP = MaxHP;
            Move(-2f, _moveSpeed);
        }

        Managers.Data.UpdateDict();

        InvokeRepeating("Regenerate", 1f, 1f);
    }

    protected override void TargetIsNull()
    {
        base.TargetIsNull();

        if (StatInfo.AttackCountdown != 0)
            StatInfo.AttackCountdown = 0;
        State = Define.State.Run;
    }

    protected override void Move(float endPosX, float moveSpeed)
    {
        float duration = Mathf.Abs(transform.position.x - endPosX) / moveSpeed;

        transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                isMove = false;
                InvokeRepeating("UpdateTarget", 0f, 0.1f);
            });
    }

    protected override void UpdateStat()
    {
        StatInfo.Coin = (int)_statDict["Coin"].statValue;
        MaxHP = _statDict["MaxHP"].statValue;
        HP = _statDict["HP"].statValue;
        Regeneration = _statDict["Regeneration"].statValue;
        StatInfo.ATK = _statDict["ATK"].statValue;
        StatInfo.DEF = _statDict["DEF"].statValue;
        AttackSpeed = _statDict["AttackSpeed"].statValue;
        StatInfo.Range = _statDict["Range"].statValue;
        StatInfo.AttackCountdown = 0;
    }

    //public void UpdateDict()
    //{
    //    _statDict["Coin"].statValue = StatInfo.Coin;
    //    _statDict["MaxHP"].statValue = MaxHP;
    //    _statDict["HP"].statValue = HP;
    //    _statDict["Regeneration"].statValue = Regeneration;
    //    _statDict["ATK"].statValue = StatInfo.ATK;
    //    _statDict["DEF"].statValue = StatInfo.DEF;
    //    _statDict["AttackSpeed"].statValue = AttackSpeed;
    //    _statDict["Range"].statValue = StatInfo.Range;

    //    //Data.StatData statData = new Data.StatData
    //    //{
    //    //    stats = new List<Data.Stat>(Managers.Data.MyPlayerStatDict.Values)
    //    //};
    //    //Managers.Data.SaveJson(statData, "MyPlayerStatDataTest");

    //    Data.GameData gameData = new Data.GameData
    //    {
    //        stats = new List<Data.Stat>(Managers.Data.MyPlayerStatDict.Values),
    //        enemys = UpdateEnemys()
    //    };
    //    Managers.Data.SaveGameData(gameData);
    //}

    //// 플레이어의 Stat에 맞게 적들 Stat도 변경할 코드 예정
    //// 다른 스크립로 이동할 수도
    //private List<Data.Enemy> UpdateEnemys()
    //{
    //    // 몬스터 체력이나 공격력을 플레이어의 스탯에 따라 변화하게 할 지
    //    // 아니면 시간? 라운드? 등에 따라 다르게 할 지 고민 중

    //    //Data.EnemyData enemyData = new Data.EnemyData
    //    //{
    //    //    enemys = new List<Data.Enemy>(Managers.Data.EnemyDict.Values)
    //    //};
    //    //Managers.Data.SaveJson(enemyData, "EnemyDataTest");

    //    return new List<Data.Enemy>(Managers.Data.EnemyDict.Values);
    //}

    public void Regenerate()
    {
        if (HP + Regeneration > MaxHP)
            return;

        HP += Regeneration;
    }

    protected override void UpdateAttacking()
    {
        base.UpdateAttacking();
        _AttackCoroutine = StartCoroutine(CheckAnimationTime(0.167f, StatInfo.ATK));
    }

    protected override void Hurt(float damage)
    {
        base.Hurt(damage);
        Managers.Data.UpdateDict("HP");
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();

        CancelInvoke("Regenerate");
        StatInfo.Coin /= 2;
        Managers.Data.UpdateDict("Coin");
    }

    protected override IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Game.Wave.RespawnPlayer();
        Managers.Resource.Destroy(gameObject);
    }
}
