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
    private Vector3 moveDirection = new Vector3(-1, 0, 0);  // �̵� ����

    private void Start()
    {
        StartScrolling();
    }

    private void StartScrolling()
    {
        // ���� ���
        Vector3 endPosition = transform.position + moveDirection * scrollAmount;

        // DOTween���� �ݺ� ���� ����
        transform.DOMove(endPosition, scrollAmount / moveSpeed)
            .SetEase(Ease.Linear) // ���� �ӵ��� �̵�
            .SetLoops(-1, LoopType.Restart) // ���� �ݺ�
            .OnStepComplete(() =>
            {
                // ��ġ �缳��
                transform.position = target.position;
            });
    }
}
