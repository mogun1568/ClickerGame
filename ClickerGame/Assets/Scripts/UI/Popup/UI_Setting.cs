using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Buttons
    {
        Button_Account,
        Button_Sound,
        Button_Close
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
        BindEvent(GetButton((int)Buttons.Button_Account).gameObject, (PointerEventData data) => { PopupAccount(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Sound).gameObject, (PointerEventData data) => { PopupSound(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);
    }

    private void PopupAccount()
    {
        Managers.UI.ShowPopupUI<UI_Login>("Popup_Login").TextInitAsync().Forget();
    }

    private void PopupSound()
    {
        //Managers.UI.ShowPopupUI<>("");
    }
}
