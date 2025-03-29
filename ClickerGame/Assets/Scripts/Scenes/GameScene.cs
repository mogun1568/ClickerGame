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

        LoadingUIAsync().Forget();
    }

    private async UniTask LoadingUIAsync()
    {
        Managers.UI.ShowPopupUI<UI_Popup>("UI_Loading");

        await Managers.Data.InitAsync();

        Managers.UI.ClosePopupUI();

        Managers.Data.OfflineReward();

        InvokeRepeating(nameof(SaveLastTime), 0f, 60f);
    }

    private void SaveLastTime()
    {
        Managers.Data.UpdateLastTime();
    }

    private float pauseStartTime = 0f;
    private const float MAX_BACKGROUND_TIME = 60f; // 60�� �̻��̸� ����
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // ��׶���� �� �� �ð� ����
            pauseStartTime = Time.realtimeSinceStartup;
        }
        else
        {
            // ���� �� ���� �ð� ���
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
