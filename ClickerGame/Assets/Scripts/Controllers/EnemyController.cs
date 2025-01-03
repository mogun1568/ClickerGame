using DG.Tweening;
using System.Collections;
using UnityEngine;

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

        Stat.HP = 10;
        Stat.ATK = 10;
        Stat.DEF = 0;
        Stat.Range = 1.5f;
        Stat.AttackSpeed = 1;
        Stat.AttackCountDown = 0;

        Move();

        _targetTag = "Player";

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }


    // 사실 상 Move() 완료 후에 하는 작업이라 Update문일 필요 없음, 수정 예정
    protected override void Update()
    {
        base.Update();

        if (_target == null)
        {
            State = Define.State.Idle;
            return;
        }

        State = Define.State.Idle;

        if (Stat.AttackCountDown <= 0)
        {
            State = Define.State.Attacking;
            Stat.AttackCountDown = 1 / Stat.AttackSpeed;
        }

        Stat.AttackCountDown -= Time.deltaTime;
    }

    private void Move()
    {
        float duration = (transform.position.x - _endPosX) / _MoveSpeed;

        transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear);
    }

    protected override void UpdateDead()
    {
        base.UpdateDead();
        Managers.Game._enemyCount--;
        //Debug.Log(Managers.Game._enemyCount);
        StartCoroutine(DeadAnim(1));
    }

    IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Destroy(gameObject);
    }
}
