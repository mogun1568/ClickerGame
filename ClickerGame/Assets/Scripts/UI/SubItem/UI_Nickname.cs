using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Nickname : UI_Base
{
    enum Texts
    {
        Text_Name
    }

    enum Buttons
    {
        Button_Rename
    }

    private void Awake()
    {
        Init();
        InitAsync().Forget();
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Button_Rename).gameObject, (PointerEventData data) => { PopupChangeName(); }, Define.UIEvent.Click);
    }

    private async UniTask InitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
        GetText((int)Texts.Text_Name).text = Managers.Data.MyPlayerInfo.Nickname;
    }

    private void PopupChangeName()
    {
        Managers.UI.ShowPopupUI<UI_ChangeName>("Popup_ChangeName").SetNickname(this);
    }

    public void Rename()
    {
        GetText((int)Texts.Text_Name).text = Managers.Data.MyPlayerInfo.Nickname;
    }
}
