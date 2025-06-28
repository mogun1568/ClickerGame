using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Managers.UI.ShowSceneUI<UI_Start>("LoginUI");

        CreateRandomBackground();
    }

    private void CreateRandomBackground()
    {
        // 랜덤 생성하려면 dict에 있는 key들로 리스트를 다시 만들어야 해서 고민 중

        // 랜덤 맵 생성 (데이터로 무슨 맵이 있는 지 저장해놔야 가능)
        Managers.Resource.Instantiate($"Map/Plain", new Vector3(0, -2, 0));

        // 랜덤 플레이어 생성
        Managers.Resource.Instantiate($"Player/LoginScene/Knight", new Vector3(-2, -0.1f, -1));
    }


    public override void Clear()
    {
        
    }
}
