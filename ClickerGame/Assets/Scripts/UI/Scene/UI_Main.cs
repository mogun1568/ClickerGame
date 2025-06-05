using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Main : UI_Scene
{
    enum Buttons
    {
        Button_Menu
    }

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_Menu).gameObject, (PointerEventData data) => { OpenMenu(); }, Define.UIEvent.Click);
    }

    private void OpenMenu()
    {
        Managers.UI.ShowPopupUI<UI_Setting>("Popup_Setting");
    }
}
