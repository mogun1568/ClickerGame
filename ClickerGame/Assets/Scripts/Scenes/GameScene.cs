using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.GamePlay;

        Managers.UI.ShowSceneUI<UI_Main>("MainUI");

        LoadingUI().Forget();
    }

    private async UniTask LoadingUI()
    {
        Managers.UI.ShowPopupUI<UI_Popup>("UI_Loading");

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        Managers.UI.ClosePopupUI();

        Managers.Data.OfflineReward();

        InvokeRepeating(nameof(SaveLastTime), 10f, 10f);
    }

    private void SaveLastTime()
    {
        Managers.Data.UpdateInfo("LastTime");
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
