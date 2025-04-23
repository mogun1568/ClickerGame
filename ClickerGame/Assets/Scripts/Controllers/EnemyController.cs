using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyController : CreatureController
{
    private Tween deadMoveTween;
    private bool _isPlayerStop; // 플레이어가 멈췄을 때 몬스터 속도 변경을 위함

    protected override async UniTask InitAsync()
    {
        await base.InitAsync();

        // (Clone)을 수정하기 전에 호출됨
        string goName = gameObject.name;
        if (goName.EndsWith("(Clone)"))
            goName = goName.Substring(0, goName.Length - 7).Trim();

        StatInfo = new EnemyStat(Managers.Data.EnemyDict[goName]);
        //SkillInfo = new Skill();
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Player";
        _endPosX = -0.51f;

        StopAllCoroutines();
        deadMoveTween.Kill();
        _isPlayerStop = false;

        Move(_endPosX, _moveSpeed);
    }

    protected override void Update()
    {
        if (State == Define.State.Death)
        {
            if (deadMoveTween != null)
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

        if (Managers.Game.MyPlayer.State == Define.State.Death)
            State = Define.State.Idle;
        else if (Managers.Game.MyPlayer.State == Define.State.Run)
        {
            if (_isPlayerStop)
                return;

            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Run;
            if (!_isPlayerStop)
            {
                _isPlayerStop = true;
                _moveSpeed = ((EnemyStat)StatInfo).MoveSpeed;
                MoveTween.Kill();
            }
        }
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
            .SetAutoKill(true)
            .OnKill(() => deadMoveTween = null)
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
