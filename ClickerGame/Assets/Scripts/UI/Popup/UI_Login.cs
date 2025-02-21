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
        {
            Managers.Firebase.OnSignIn();
        }
        else
        {
            Managers.Firebase.OnSignOut();
        }
    }
}
