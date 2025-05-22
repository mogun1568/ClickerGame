using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopItem : UI_Base
{
    enum Buttons
    {
        Purchase_Button
    }

    enum Images
    {
        Icon_Item
    }

    enum Texts
    {
        Text_ItemName,
        Text_ItemInfo,
        Text_ItemPrice
    }

    private string _goName;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        _goName = gameObject.name;

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Purchase_Button).gameObject, (PointerEventData data) => { PurchaseItem(); }, Define.UIEvent.Click);
    }

    private void PurchaseItem()
    {
        switch (_goName)
        {
            case "Reincarnation":
                Reincarnation();
                break;
            case "GiveUp":
                GiveUp();
                break;
        }
    }

    private void Reincarnation()
    {
        // 조건 추가 예정
        if (Managers.Data.MyPlayerInfo.Round < 100)
            return;

        Managers.Data.DoReincarnation(true);
    }

    private void GiveUp()
    {
        Managers.Data.DoReincarnation(false);
    }
}
