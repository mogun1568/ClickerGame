using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopItem : UI_Base
{
    enum Buttons
    {
        Button_Purchase
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
    private int _addCoin = 1000;

    // Start
    void Awake()
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
        BindEvent(GetButton((int)Buttons.Button_Purchase).gameObject, (PointerEventData data) => { PurchaseItem(); }, Define.UIEvent.Click);

        DataInit();
    }

    // 상점 데이터도 ScriptableObject로 관리할까 고민 중
    private void DataInit()
    {
        string itemName = "", itemInfo = ""; //itemPrice = "";

        switch (_goName)
        {
            case "Reincarnation":
                itemName = "환생";
                itemInfo = "기초 능력이 조금 상향된 채로 \n다시 태어납니다.";
                break;
            case "GiveUp":
                itemName = "포기";
                itemInfo = "기초 능력 그대로 다시 \n태어납니다.";
                break;
            case "AddCoin":
                itemName = "코인 구매";
                itemInfo = $"광고를 시청하고 \n{_addCoin} 코인를 얻습니다.";
                break;
        }

        GetText((int)Texts.Text_ItemName).text = itemName;
        GetText((int)Texts.Text_ItemInfo).text = itemInfo;
        //GetText((int)Texts.Text_ItemPrice).text = itemPrice;
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
            case "AddCoin":
                AddCoin();
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
        // 광고

        Managers.Data.DoReincarnation(false);
    }

    // 하루 횟수 제한 필요
    private void AddCoin()
    {
        // 광고

        Managers.Game.MyPlayer.StatInfo.Coin += _addCoin;
    }
}
