using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Managers.UI.ShowSceneUI<UI_Start>("LoginUI");

        CreateRandomBackground();

        Managers.Sound.Play("Adventure's Spirit - Title Theme", Define.Sound.BGM);
    }

    private void CreateRandomBackground()
    {
        // ���� �����Ϸ��� dict�� �ִ� key��� ����Ʈ�� �ٽ� ������ �ؼ� ��� ��

        // ���� �� ���� (�����ͷ� ���� ���� �ִ� �� �����س��� ����)
        Managers.Resource.Instantiate($"Map/Plain", new Vector3(0, -2, 0));

        // ���� �÷��̾� ����
        Managers.Resource.Instantiate($"Player/LoginScene/Knight", new Vector3(-2, -0.1f, -1));
    }


    public override void Clear()
    {
        
    }
}
