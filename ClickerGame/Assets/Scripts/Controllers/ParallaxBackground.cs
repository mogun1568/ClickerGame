using DG.Tweening;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float scrollAmount = 15;                        // 이어지는 두 배경 사이의 거리
    [SerializeField]
    private float moveSpeed;                                // 이동 속도

    private Tween scrollTween;
    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        startPos = new Vector3(scrollAmount, transform.position.y, transform.position.z);
        endPos = new Vector3(-scrollAmount, transform.position.y, transform.position.z);

        StartScrolling();
    }

    private void StartScrolling()
    {
        float distance = Vector3.Distance(transform.position, endPos);
        float duration = distance / moveSpeed;

        scrollTween = transform.DOMove(endPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = startPos;
                StartScrolling(); // 반복
            });
    }

    private void Update()
    {
        if (!Managers.Data.GameDataReady)
            return;

        if (Managers.Game.MyPlayer == null)
            return;

        if (Managers.Game.MyPlayer._onlyPlayerMove)
        {
            if (scrollTween.IsPlaying()) scrollTween.Pause();
            return;
        }

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
