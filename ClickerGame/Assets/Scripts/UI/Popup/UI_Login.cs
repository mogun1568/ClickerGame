using Cysharp.Threading.Tasks;
using TMPro;
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
        Text_Login,
        Text_Info
    }

    private string _loginText, _infoText;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

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
            Login();
        else
            Managers.Firebase.OnSignOut();
    }

    private void Login()
    {
        string str = "연동 계정의 데이터가 존재할 경우 게스트 데이터가 삭제됩니다. 계속하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_Notice>("Popup_Notice").UIInit(str);
    }

    public async UniTask TextInitAsync()
    {
        //if (_infoText != "")
        //    return;

        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);

        if (Managers.Firebase.auth.CurrentUser == null)
        {
            _loginText = "GOOGLE 로그인";
            _infoText = "로그인되어 있지 않습니다.";
        }
        else
        {
            _loginText = "GOOGLE 로그아웃";
            _infoText = "환영합니다";
        }

        GetText((int)Texts.Text_Login).text = _loginText;
        GetText((int)Texts.Text_Info).text = _infoText;
    }
}
