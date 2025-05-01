using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyController : CreatureController
{
    private Tween deadMoveTween;
    private bool _playerFirstAttack; // 플레이어가 첫타를 때렸는 지 확인
    private bool _isPlayerMove;

    protected override async UniTask InitAsync()
    {
        await base.InitAsync();

        // (Clone)을 수정하기 전에 호출됨
        string goName = gameObject.name;
        if (goName.EndsWith("(Clone)"))
            goName = goName.Substring(0, goName.Length - 7).Trim();

        StatInfo = new EnemyStat(Managers.Data.EnemyDict[goName]);
        SkillInfo = GetComponent<Skill>();
        SkillInfo.Init();
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Player";
        _endPosX = -0.51f;

        StopAllCoroutines();
        deadMoveTween.Kill();
        _playerFirstAttack = false;
        _isPlayerMove = false;

        Move(_endPosX, _moveSpeed, Define.TweenType.Idle);
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
        {
            // 넉백 후 오는 중에 플레이어가 죽으면 Idle 상태로 이동할 수도
            State = Define.State.Idle;
            return;
        }

        if (Managers.Game.MyPlayer.State == Define.State.Run)
        {
            if (_playerFirstAttack)
            {
                if (!_isPlayerMove)
                {
                    _isPlayerMove = true;
                    Move(_endPosX, _moveSpeed + _defaultMoveSpeed, Define.TweenType.Run);
                }

                return;
            }

            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Run;

            if (!_playerFirstAttack)
            {
                _playerFirstAttack = true;
                _moveSpeed = ((EnemyStat)StatInfo).MoveSpeed;
                Move(_endPosX, _moveSpeed, Define.TweenType.Run);
            }
            _isPlayerMove = false;
        }
    }

    private void DeadMove(float endPosX, float moveSpeed)
    {
        if (MoveTween != null)
        {
            MoveTween.Kill();
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
        Managers.Skill.RandomAddSkill();

        DeadMove(-7f, _defaultMoveSpeed);
    }
}