using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Popup
{
    enum Buttons
    {
        Button_Log,
        Button_Close,
        Text_DeleteAccount
    }

    enum Texts
    {
        Text_Login,
        Text_Info
    }

    private string _loginText, _infoText;
    private string _deleteAccountUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdefxubR3y8fGUWwL-D8wumlA-7M0dHRIk3CttV8n2W6fM-dg/viewform?usp=header";

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
        BindEvent(GetButton((int)Buttons.Text_DeleteAccount).gameObject, (PointerEventData data) => { DeleteAccount(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);

        GetButton((int)Buttons.Text_DeleteAccount).gameObject.SetActive(false);

        // �Ƹ� �α��� ���� �Լ����� �񵿱�� �ʰ� ���ŵɰ� ������ ���� ���� ��� ��
        //if (Managers.Firebase.auth.CurrentUser == null)
        //    GetText((int)Texts.Text_Info).text = "�α��εǾ� ���� �ʽ��ϴ�.";
        //else
        //    GetText((int)Texts.Text_Info).text = "ȯ���մϴ�!";

        TextInit();
    }

    public void TextInit()
    {
        if (Managers.Firebase.GoogleLogIn)
        {
            _loginText = "GOOGLE �α׾ƿ�";
            _infoText = "ȯ���մϴ�";
            GetButton((int)Buttons.Text_DeleteAccount).gameObject.SetActive(true);
        }
        else
        {
            _loginText = "GOOGLE �α���";
            _infoText = "�α��εǾ� ���� �ʽ��ϴ�.";
        }

        GetText((int)Texts.Text_Login).text = _loginText;
        GetText((int)Texts.Text_Info).text = _infoText;
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

    private void DeleteAccount()
    {
        Application.OpenURL(_deleteAccountUrl);
    }
}
