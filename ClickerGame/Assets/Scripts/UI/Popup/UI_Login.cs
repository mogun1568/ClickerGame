using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Popup
{
    enum Buttons
    {
        Button_Log,
        Button_Close
    }

    enum Texts
    {
        Text_Info
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_Log).gameObject, (PointerEventData data) => { Log(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);

        // 아마 로그인 관련 함수들이 비동기라 늦게 갱신될거 같은데 어케 할지 고민 중
        //if (Managers.Firebase.auth.CurrentUser == null)
        //    GetText((int)Texts.Text_Info).text = "로그인되어 있지 않습니다.";
        //else
        //    GetText((int)Texts.Text_Info).text = "환영합니다!";
    }

    private void Log()
    {
        if (Managers.Firebase.auth.CurrentUser == null)
        {
            Managers.Firebase.OnSignIn();
        }
        else
        {
            Managers.Firebase.OnSignOut();
        }
    }
}
