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
    public bool isMove;

    public float Regeneration
    {
        get { return ((PlayerStat)StatInfo).Regeneration; }
        set { ((PlayerStat)StatInfo).Regeneration = value; }
    }

    protected override async UniTask InitAsync()
    {
        await base.InitAsync();

        Managers.Game.MyPlayer = this;

        //State = Define.State.Run;

        StatInfo = new PlayerStat(Managers.Data.MyPlayerStatDict);
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Enemy";

        if (transform.position.x == -2)
        {
            isMove = false;
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
        }
        else
        {
            isMove = true;
            HP = MaxHP;
            Move(-2f, _moveSpeed);
        }

        InvokeRepeating(nameof(Regenerate), 1f, 1f);
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
                InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
            });
    }

    public void Regenerate()
    {
        if (HP == MaxHP)
            return;

        HP += Regeneration;
    }

    protected override void UpdateAttacking()
    {
        base.UpdateAttacking();
        _AttackCoroutine = StartCoroutine(CheckAnimationTime(0.167f, StatInfo.ATK));
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();

        CancelInvoke(nameof(Regenerate));
        StatInfo.Coin /= 2;
    }

    protected override IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Game.Wave.RespawnPlayer();
        Managers.Resource.Destroy(gameObject);
    }
}
