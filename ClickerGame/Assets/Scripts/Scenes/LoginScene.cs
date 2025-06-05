public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Managers.UI.ShowSceneUI<UI_Start>("LoginUI");
    }

    public override void Clear()
    {
        
    }
}
