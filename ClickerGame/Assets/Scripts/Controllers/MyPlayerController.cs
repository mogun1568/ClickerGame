using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class MyPlayerController : CreatureController
{
    // �÷��̾ �׾��� �� ��� ������ ����
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

        // Stat�� _maxHP�� ���� �ֱ� ����, ������ ������ �ڵ� Ǯ�� ��ɵ� ��
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

        // �ʱ�ȭ�ϸ� �ʹ� ��ⰰ�Ƽ� ��� ��
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
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        Managers.UI.ShowPopupUI<UI_Resurrection>("Popup_Resurrection");
        Managers.Resource.Destroy(gameObject);
    }
}
