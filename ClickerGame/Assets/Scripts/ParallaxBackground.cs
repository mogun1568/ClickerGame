using DG.Tweening;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private Transform target;                               // 현재 배경과 이어지는 배경
    [SerializeField]
    private float scrollAmount = 16;                        // 이어지는 두 배경 사이의 거리
    [SerializeField]
    private float moveSpeed;                                // 이동 속도
    [SerializeField]
    private Vector3 moveDirection = new Vector3(-1, 0, 0);  // 이동 방향

    private void Start()
    {
        StartScrolling();
    }

    private void StartScrolling()
    {
        // 끝점 계산
        Vector3 endPosition = transform.position + moveDirection * scrollAmount;

        // DOTween으로 반복 동작 설정
        transform.DOMove(endPosition, scrollAmount / moveSpeed)
            .SetEase(Ease.Linear) // 일정 속도로 이동
            .SetLoops(-1, LoopType.Restart) // 무한 반복
            .OnStepComplete(() =>
            {
                // 위치 재설정
                transform.position = target.position;
            });
    }
}
