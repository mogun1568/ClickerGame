using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CreatureController
{
    [SerializeField]
    private float _endPosX = -0.5f;
    [SerializeField]
    private float _moveSpeed = 2.5f;
    private Tween MoveTween;
    private Tween deadMoveTween;
    private Data.Enemy _enemyStat;

    protected override void Init()
    {
        base.Init();

        transform.position = new Vector3(7, 1.9f, -1);

        // (Clone)을 수정하기 전에 호출됨
        string goName = gameObject.name;
        if (goName.EndsWith("(Clone)"))
            goName = goName.Substring(0, goName.Length - 7).Trim();

        _enemyStat = Managers.Data.EnemyDict[goName];
        UpdateStat();
        StatInfo.AttackCountdown = 0;

        _targetTag = "Player";
        MoveTween = null;
        deadMoveTween = null;

        Move();
    }

    protected override void UpdateStat()
    {
        StatInfo.HP = _enemyStat.enemyMaxHP;
        StatInfo.ATK = _enemyStat.enemyATK;
        StatInfo.DEF = _enemyStat.enemyDEF;
        AttackSpeed = _enemyStat.enemyAttackSpeed;
        StatInfo.Range = _enemyStat.enemyRange;
        StatInfo.Coin = _enemyStat.enemyCoin;
    }

    protected override void Update()
    {
        if (State == Define.State.Death)
        {
            if (Managers.Game.MyPlayer.State == Define.State.Run)
            {
                if (!deadMoveTween.IsPlaying()) deadMoveTween.Play();
            }
            else
            {
                if (deadMoveTween.IsPlaying()) deadMoveTween.Pause();
            }
        }

        base.Update();

        if (MoveTween != null)
        {
            if (State != Define.State.Idle)
            {
                if (MoveTween.IsPlaying()) MoveTween.Pause();
            }
            else
            {
                if (!MoveTween.IsPlaying()) MoveTween.Play();
            }
        }
    }

    private void Move()
    {
        _endPosX = -0.5f;
        float duration = (transform.position.x - _endPosX) / _moveSpeed;

        MoveTween = transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                InvokeRepeating("UpdateTarget", 0f, 0.1f);
                MoveTween = null;
            });
    }

    private void DeadMove()
    {
        if (MoveTween != null)
        {
            MoveTween.Kill();
            MoveTween = null;
        }

        _endPosX = -7;
        float duration = (transform.position.x - _endPosX) / _moveSpeed;

        deadMoveTween = transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear)
            .Pause();
    }

    protected override void UpdateAttacking()
    {
        base.UpdateHurt();
        _AttackCoroutine = StartCoroutine(CheckAnimationTime(0.5f, StatInfo.ATK));
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();

        Managers.Game._enemyCount--;
        //Debug.Log(Managers.Game._enemyCount);

        Managers.Game.MyPlayer.StatInfo.Coin += StatInfo.Coin;
        Managers.Game.MyPlayer.UpdateDict();
        Debug.Log(Managers.Game.MyPlayer.StatInfo.Coin);

        DeadMove();
        StartCoroutine(DeadAnim(1));
    }

    IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Destroy(gameObject);
    }
}
