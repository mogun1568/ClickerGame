using DG.Tweening;
using UnityEngine;

public class EnemyController : CreatureController
{
    [SerializeField]
    private float endPosX = -0.5f;
    [SerializeField]
    private float MoveSpeed = 2.5f;

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        float duration = (transform.position.x - endPosX) / MoveSpeed;

        transform.DOMoveX(endPosX, duration)
            .SetEase(Ease.Linear);
    }
}
