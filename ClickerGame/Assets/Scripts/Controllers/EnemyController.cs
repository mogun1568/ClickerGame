using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyController : CreatureController
{
    [SerializeField]
    private float _endPosX = -0.5f;
    [SerializeField]
    private float _MoveSpeed = 2.5f;
    private bool _completeMove;
    private Tween deadMoveTween;

    protected override void Init()
    {
        base.Init();

        transform.position = new Vector3(7, 1.9f, -1);

        Stat.HP = 50;
        Stat.ATK = 0;
        Stat.DEF = 0;
        Stat.AttackSpeed = 1;
        Stat.AttackCountdown = 0;
        Stat.Range = 1.5f;

        _targetTag = "Player";
        _completeMove = false;
        deadMoveTween = null;

        Move();
    }

    protected override void Update()
    {
        if (!_completeMove)
            return;

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
    }

    private void Move()
    {
        _endPosX = -0.5f;
        float duration = (transform.position.x - _endPosX) / _MoveSpeed;

        transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 이동 완료 시 호출
                InvokeRepeating("UpdateTarget", 0f, 0.1f);
                _completeMove = true;
            });
    }

    private void DeadMove()
    {
        _endPosX = -7;
        float duration = (transform.position.x - _endPosX) / _MoveSpeed;

        deadMoveTween = transform.DOMoveX(_endPosX, duration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)   // 자동 삭제 방지 (Pause 이후 다시 사용하려면 필요)
            .Pause();
    }

    protected override void UpdateDie()
    {
        base.UpdateDie();
        Managers.Game._enemyCount--;
        //Debug.Log(Managers.Game._enemyCount);
        DeadMove();
        StartCoroutine(DeadAnim(1));
    }

    IEnumerator DeadAnim(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Destroy(gameObject);
    }
}
