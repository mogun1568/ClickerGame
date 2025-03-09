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

        InvokeRepeating(nameof(SaveLastTime), 10f, 10f);
    }

    private void SaveLastTime()
    {
        Managers.Data.UpdateInfo("LastTime");
    }

    public override void Clear()
    {
        
    }
}
