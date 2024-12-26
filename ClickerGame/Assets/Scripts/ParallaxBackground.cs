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
    private float moveDirX = -1;  // 이동 방향

    private Tween scrollTween;

    private void Start()
    {
        StartScrolling();
    }

    private void StartScrolling()
    {
        // 끝점 계산
        float endPosX = transform.position.x + moveDirX * scrollAmount;

        // DOTween으로 트윈 생성
        scrollTween = transform.DOMoveX(endPosX, scrollAmount / moveSpeed)
            .SetEase(Ease.Linear) // 일정 속도로 이동
            .SetLoops(-1, LoopType.Restart) // 무한 반복
            .OnStepComplete(() =>
            {
                // 위치 재설정
                transform.position = target.position;
            });

        // 처음에는 정지 상태로 시작
        scrollTween.Pause();
    }

    // 감속 기능(추후 추가)
    /*private void StartDeceleration()
    {
        // 끝점 계산
        float endPosX = transform.position.x + moveDirX * moveSpeed;

        // DOTween으로 트윈 생성
        scrollTween = transform.DOMoveX(endPosX, 1f)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // 위치 재설정
                //transform.position = target.position;
            });
    }*/

    private void Update()
    {
        // CanMove 상태에 따라 트윈 재생 또는 일시 정지
        if (Managers.Game.MyPlayer.State == Define.State.Run)
        {
            if (!scrollTween.IsPlaying()) scrollTween.Play();
        }
        else
        {
            if (scrollTween.IsPlaying()) scrollTween.Pause();
        }
    }
}
