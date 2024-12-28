using DG.Tweening;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : CreatureController
{
    [SerializeField]
    private float _endPosX = -0.5f;
    [SerializeField]
    private float _MoveSpeed = 2.5f;

    protected override void Init()
    {
        base.Init();

        transform.position = new Vector3(7, 1.9f, -1);

        State = Define.State.Idle;

        Stat.HP = 20;
        Stat.ATK = 10;
        Stat.DEF = 0;
        Stat.Range = 1.5f;
        Stat.AttackSpeed = 1;
        Stat.AttackCountDown = 1;

        Move();
    }

    private void Move()
    {
        float duration = (transform.position.x - _endPosX) / _MoveSpeed;

        transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear);
    }

    protected override void Die()
    {
        base.Die();
        Managers.Game._enemyCount--;
        Managers.Resource.Destroy(this.gameObject);
    }
}
