using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyController : CreatureController
{
    private Tween deadMoveTween;
    private bool _playerFirstAttack; // 플레이어가 첫타를 때렸는지 확인

    private string ChangeName()
    {
        string goName = gameObject.name;

        // (Clone)을 수정하기 전에 호출됨
        if (goName.EndsWith("(Clone)"))
            goName = goName.Substring(0, goName.Length - 7).Trim();

        if (goName.EndsWith("Bandit"))
            goName = "Bandit";

        return goName;
    }

    protected override async UniTask InitAsync()
    {
        await base.InitAsync();

        string goName = ChangeName();

        State = Define.State.Idle;
        StatInfo = new EnemyStat(goName);
        SkillInfo = GetComponent<Skill>();
        SkillInfo.Init();
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Player";
        _endPosX = -0.51f;

        StopAllCoroutines();
        deadMoveTween.Kill();
        _playerFirstAttack = false;
        _moveSpeed = ((EnemyStat)StatInfo).MoveSpeed;

        Move(_endPosX, _backgroundMoveSpeed, Define.TweenType.Idle);
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
    }

    protected override void TargetIsNull()
    {
        base.TargetIsNull();

        if (Managers.Game.MyPlayer.State == Define.State.Death)
        {
            if (MoveTween == null) 
                State = Define.State.Idle;
            return;
        }

        if (Managers.Game.MyPlayer._onlyPlayerMove)
            return;

        if (_tweenType == Define.TweenType.Knockback)
            return;

        if (_playerFirstAttack)
            State = Define.State.Run;

        if (Managers.Game.MyPlayer.State == Define.State.Run)
        {
            if (!_playerFirstAttack)
                return;

            if (_debuff == Define.Debuff.Slow)
                Move(_endPosX, _moveSpeed + _backgroundMoveSpeed, Define.TweenType.Slow);
            else
                Move(_endPosX, _moveSpeed + _backgroundMoveSpeed, Define.TweenType.Run);
        }
        else
        {
            _playerFirstAttack = true;

            if (_debuff == Define.Debuff.Slow)
                Move(_endPosX, _moveSpeed, Define.TweenType.Slow);
            else
                Move(_endPosX, _moveSpeed, Define.TweenType.Run);
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

    protected override void Hurt(float damage)
    {
        base.Hurt(damage);
        _playerFirstAttack = true;
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();

        Managers.Game.MyPlayer.StatInfo.Coin += StatInfo.Coin;
        Managers.Game.Wave._enemyCount--;
        //Logging.Log(Managers.Game.Wave._enemyCount);
        Managers.Skill.RandomAddSkill();

        DeadMove(-7f, _backgroundMoveSpeed);
    }
}