using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float scrollAmount = 15;                        // �̾����� �� ��� ������ �Ÿ�
    [SerializeField]
    private float moveSpeed;                                // �̵� �ӵ�

    private Tween scrollTween;
    Vector3 startPos;
    Vector3 endPos;

    private string _mapName;

    void Start()
    {
        InitAsync().Forget();
    }

    private async UniTask InitAsync()
    {
        MoveSpeedInit();

        // �α��� ȭ���� �ƴ� ����
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.Login)
        {
            await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
            _mapName = Managers.Data.MyPlayerInfo.Map;
        }

        startPos = new Vector3(scrollAmount, transform.position.y, transform.position.z);
        endPos = new Vector3(-scrollAmount, transform.position.y, transform.position.z);

        StartScrolling();
    }

    private void MoveSpeedInit()
    {
        if (name.StartsWith("Layer"))
        {
            char lastChar = name[name.Length - 1];  // �������� ������ ����
            int digit = int.Parse(lastChar.ToString());  // ���� �� ���� ��ȯ
            moveSpeed = 0.2f * digit;
        }
        else
            moveSpeed = 2f;
    }

    private void StartScrolling()
    {
        // �α��� ȭ���� �ƴ� ����
        if (Managers.Scene.CurrentScene.SceneType != Define.Scene.Login)
        {
            if (_mapName != Managers.Data.MyPlayerInfo.Map)
            {
                scrollTween = null;
                return;
            }
        } 

        float distance = Vector3.Distance(transform.position, endPos);
        float duration = distance / moveSpeed;

        scrollTween = transform.DOMove(endPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = startPos;
                StartScrolling(); // �ݺ�
            });
    }

    private void Update()
    {
        if (!Managers.Data.GameDataReady)
            return;

        if (Managers.Game.MyPlayer == null)
            return;

        if (scrollTween == null)
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
