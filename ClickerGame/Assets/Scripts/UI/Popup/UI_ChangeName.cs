using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

public class UI_ChangeName : UI_Popup
{
    enum GameObjects
    {
        InputField_NickName
    }

    enum Buttons
    {
        Button_Close,
        Button_Change
    }

    private UI_Nickname _nickname;
    private TMP_InputField inputField;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        inputField = GetObject((int)GameObjects.InputField_NickName).GetComponent<TMP_InputField>();
        inputField.text = Managers.Data.MyPlayerInfo.Nickname;

        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Change).gameObject, (PointerEventData data) => { ChangeName(); }, Define.UIEvent.Click);
    }

    public void SetNickname(UI_Nickname nickname)
    {
        _nickname = nickname;
    }

    private void ChangeName()
    {
        string newName = inputField.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            Logging.Log("이름을 입력해주세요!");
            return;
        }

        Managers.Data.MyPlayerInfo.Nickname = newName;
        _nickname.Rename();
        ClosePopupUI();
    }
}
