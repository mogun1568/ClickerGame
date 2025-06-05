using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameQuit : UI_Popup
{
    enum Buttons
    {
        Button_Quit,
        Button_Cancel
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
        BindEvent(GetButton((int)Buttons.Button_Quit).gameObject, (PointerEventData data) => { GameQuit(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Cancel).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);
    }

    private void GameQuit()
    {
        Managers.Data.SaveGameData();
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
