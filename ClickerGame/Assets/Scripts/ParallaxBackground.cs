using DG.Tweening;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private Transform target;                               // ���� ���� �̾����� ���
    [SerializeField]
    private float scrollAmount = 16;                        // �̾����� �� ��� ������ �Ÿ�
    [SerializeField]
    private float moveSpeed;                                // �̵� �ӵ�
    [SerializeField]
    private float moveDirX = -1;  // �̵� ����

    private Tween scrollTween;

    private void Start()
    {
        StartScrolling();
    }

    private void StartScrolling()
    {
        // ���� ���
        float endPosX = transform.position.x + moveDirX * scrollAmount;

        // DOTween���� Ʈ�� ����
        scrollTween = transform.DOMoveX(endPosX, scrollAmount / moveSpeed)
            .SetEase(Ease.Linear) // ���� �ӵ��� �̵�
            .SetLoops(-1, LoopType.Restart) // ���� �ݺ�
            .OnStepComplete(() =>
            {
                // ��ġ �缳��
                transform.position = target.position;
            });

        // ó������ ���� ���·� ����
        scrollTween.Pause();
    }

    private void Update()
    {
        // CanMove ���¿� ���� Ʈ�� ��� �Ǵ� �Ͻ� ����
        if (Managers.Game.CanMove)
        {
            if (!scrollTween.IsPlaying()) scrollTween.Play();
        }
        else
        {
            if (scrollTween.IsPlaying()) scrollTween.Pause();
        }
    }
}
