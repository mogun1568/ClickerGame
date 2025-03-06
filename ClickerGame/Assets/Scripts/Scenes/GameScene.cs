using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Main>("MainUI");

        LoadingUI().Forget();
    }

    private async UniTask LoadingUI()
    {
        Managers.UI.ShowPopupUI<UI_Popup>("UI_Loading");

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        Managers.UI.ClosePopupUI();
    }

    public override void Clear()
    {
        
    }
}
