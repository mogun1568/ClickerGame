using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class MyPlayerController : CreatureController
{
    // 플레이어가 죽었을 때 배경 움직임 위함
    public bool _onlyPlayerMove;

    public float Regeneration
    {
        get { return ((PlayerStat)StatInfo).Regeneration; }
        set { ((PlayerStat)StatInfo).Regeneration = value; }
    }

    protected override async UniTask InitAsync()
    {
        await base.InitAsync();

        Managers.Game.MyPlayer = this;

        State = Define.State.Run;
        StatInfo = new PlayerStat(Managers.Data.MyPlayerStatDict);
        SkillInfo = GetComponent<PlayerSkill>();
        SkillInfo.Init();
        _animator.SetFloat("AttackSpeed", AttackSpeed);

        _targetTag = "Enemy";
        StopAllCoroutines();

        // Stat의 _maxHP에 값을 주기 위함, 스폰할 때마다 자동 풀피 기능도 됨
        MaxHP = Managers.Data.MyPlayerStatDict["MaxHP"].statValue;

        if (transform.position.x == -2)
        {
            _onlyPlayerMove = false;
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
        }
        else
        {
            _onlyPlayerMove = true;
            _endPosX = -2f;
            Move(_endPosX, _backgroundMoveSpeed, Define.TweenType.Run);
        }

        InvokeRepeating(nameof(Regenerate), 1f, 1f);
    }

    protected override void TargetIsNull()
    {
        base.TargetIsNull();

        // 초기화하면 너무 사기같아서 고민 중
        //if (StatInfo.AttackCountdown != 0)
        //    StatInfo.AttackCountdown = 0;
        State = Define.State.Run;
    }

    protected override void TweenComplete()
    {
        _onlyPlayerMove = false;
        base.TweenComplete();
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

    protected override void Skill()
    {
        base.Skill();
        ((PlayerSkill)SkillInfo).UseSkill(_target);
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();
        CancelInvoke(nameof(Regenerate));
    }

    protected override IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.UI.ShowPopupUI<UI_Resurrection>("Popup_Resurrection");
        Managers.Resource.Destroy(gameObject);
    }
}
