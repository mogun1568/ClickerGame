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

    void Start()
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
            })
            .Pause();
    }

    // ���� ���(���� �߰�)
    /*private void StartDeceleration()
    {
        // ���� ���
        float endPosX = transform.position.x + moveDirX * moveSpeed;

        // DOTween���� Ʈ�� ����
        scrollTween = transform.DOMoveX(endPosX, 1f)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                // ��ġ �缳��
                //transform.position = target.position;
            });
    }*/

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
