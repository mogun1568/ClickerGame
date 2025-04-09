using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Notice : UI_Popup
{
    enum Texts
    {
        Text_Info
    }

    enum Buttons
    {
        Button_Confirm,
        Button_Cancel
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Confirm).gameObject, (PointerEventData data) => { Confirm(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Cancel).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);
    }

    private void Confirm()
    {
        // �α��� �˸����θ� �� �� ��� ��
        Managers.UI.CloseAllPopupUI();
        Managers.Firebase.OnSignIn();
    }

    public void UIInit(string str)
    {
        GetText((int)Texts.Text_Info).text = str;
    }
}
