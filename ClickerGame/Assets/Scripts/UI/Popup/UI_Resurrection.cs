using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Resurrection : UI_Popup
{
    enum Buttons
    {
        Button_AdResurrection,
        Button_Resurrection
    }

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_AdResurrection).gameObject, (PointerEventData data) => { Advertising(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Resurrection).gameObject, (PointerEventData data) => { NoAdvertising(); }, Define.UIEvent.Click);
    }

    private void Advertising()
    {
        // ����

        Resurrection();
    }

    private void NoAdvertising()
    {
        Managers.Game.MyPlayer.StatInfo.Coin /= 2;
        Resurrection();
    }

    private void Resurrection()
    {
        
        Managers.Game.Wave.RespawnPlayer();
        Managers.UI.ClosePopupUI();
    }
}
