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

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Log).gameObject, (PointerEventData data) => { Log(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);

        // �Ƹ� �α��� ���� �Լ����� �񵿱�� �ʰ� ���ŵɰ� ������ ���� ���� ��� ��
        //if (Managers.Firebase.auth.CurrentUser == null)
        //    GetText((int)Texts.Text_Info).text = "�α��εǾ� ���� �ʽ��ϴ�.";
        //else
        //    GetText((int)Texts.Text_Info).text = "ȯ���մϴ�!";
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
        string str = "���� ������ �����Ͱ� ������ ��� �Խ�Ʈ �����Ͱ� �����˴ϴ�. ����Ͻðڽ��ϱ�?";
        Managers.UI.ShowPopupUI<UI_Notice>("Popup_Notice").UIInit(str);
    }

    public async UniTask TextInitAsync()
    {
        //if (_infoText != "")
        //    return;

        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);

        if (Managers.Firebase.auth.CurrentUser == null)
        {
            _loginText = "GOOGLE �α���";
            _infoText = "�α��εǾ� ���� �ʽ��ϴ�.";
        }
        else
        {
            _loginText = "GOOGLE �α׾ƿ�";
            _infoText = "ȯ���մϴ�";
        }

        GetText((int)Texts.Text_Login).text = _loginText;
        GetText((int)Texts.Text_Info).text = _infoText;
    }
}
