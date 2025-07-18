using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.GamePlay;

        Managers.UI.ShowSceneUI<UI_Main>("MainUI");
        Managers.UI.ToastMessage = Managers.Resource.Instantiate("UI/SubItem/ToastMessage").GetComponent<UI_ToastMessage>();

        LoadingUIAsync().Forget();
    }

    private async UniTask LoadingUIAsync()
    {
        await Managers.Data.InitAsync();
        await UniTask.WaitUntil(() => Managers.Game.GameStartReady);

        if (Managers.Scene.loading.isShow)
            Managers.Scene.loading.Hide();

        //Managers.Sound.Play("bgm1", Define.Sound.BGM);

        Managers.Data.OfflineReward();

        InvokeRepeating(nameof(SaveLastTime), 0f, 60f);
    }

    private void SaveLastTime()
    {
        Managers.Data.UpdateLastTime();
    }

    private float pauseStartTime = 0f;
    private const float MAX_BACKGROUND_TIME = 60f; // 60초 이상이면 종료
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // 백그라운드로 갈 때 시간 저장
            pauseStartTime = Time.realtimeSinceStartup;
        }
        else
        {
            // 복귀 시 지난 시간 계산
            float elapsedTime = Time.realtimeSinceStartup - pauseStartTime;

            if (elapsedTime >= MAX_BACKGROUND_TIME)
                Managers.Scene.LoadScene(Define.Scene.GamePlay);
        }
    }

    private void OnApplicationQuit()
    {
        SaveLastTime();
    }

    public override void Clear()
    {
        
    }
}
