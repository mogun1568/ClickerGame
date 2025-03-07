using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Main : UI_Scene
{
    enum Buttons
    {
        Button_Menu
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Menu).gameObject, (PointerEventData data) => { OpenMenu(); }, Define.UIEvent.Click);
    }

    private void OpenMenu()
    {
        //Managers.UI.ShowPopupUI<UI_Login>("Popup_Login");
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }
}
