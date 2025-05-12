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
        Icon_Shop
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
        }
    }

    private void Reincarnation()
    {
        Managers.Data.DoReincarnation();
    }
}
