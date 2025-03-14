using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CreatureController
{
    private Tween MoveTween;
    private Tween deadMoveTween;
    private Data.Enemy _enemyStat;

    protected override async UniTask Init()
    {
        await base.Init();

        // (Clone)을 수정하기 전에 호출됨
        string goName = gameObject.name;
        if (goName.EndsWith("(Clone)"))
            goName = goName.Substring(0, goName.Length - 7).Trim();

        StatInfo = new EnemyStat(Managers.Data.EnemyDict[goName]);
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Player";
        StopAllCoroutines();
        MoveTween = null;
        deadMoveTween = null;

        Move(-0.5f, _moveSpeed);
    }

    protected override void UpdateInfoAndStat()
    {
        MaxHP = _enemyStat.enemyMaxHP;
        HP = _enemyStat.enemyMaxHP;
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
            if (State == Define.State.Hurt)
            {
                if (MoveTween.IsPlaying()) MoveTween.Pause();
            }
            else
            {
                if (!MoveTween.IsPlaying()) MoveTween.Play();
            }
        }
    }

    protected override void TargetIsNull()
    {
        base.TargetIsNull();

        if (Managers.Game.MyPlayer.State == Define.State.Run || Managers.Game.MyPlayer.State == Define.State.Death)
            State = Define.State.Idle;
        else
            State = Define.State.Run;
    }

    protected override void Move(float endPosX, float moveSpeed)
    {
        float duration = Mathf.Abs(transform.position.x - endPosX) / moveSpeed;

        MoveTween = transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
                MoveTween = null;
            });
    }

    private void DeadMove(float endPosX, float moveSpeed)
    {
        if (MoveTween != null)
        {
            MoveTween.Kill();
            MoveTween = null;
        }

        float duration = (transform.position.x - endPosX) / moveSpeed;

        deadMoveTween = transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear)
            .Pause();
    }

    protected override void UpdateAttacking()
    {
        base.UpdateAttacking();
        _AttackCoroutine = StartCoroutine(CheckAnimationTime(0.5f, StatInfo.ATK));
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();

        Managers.Game.Wave._enemyCount--;
        Managers.Game.MyPlayer.StatInfo.Coin += StatInfo.Coin;

        DeadMove(-7f, _moveSpeed);
    }
}
