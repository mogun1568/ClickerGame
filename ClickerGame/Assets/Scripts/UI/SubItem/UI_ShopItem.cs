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

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Purchase).gameObject, (PointerEventData data) => { PurchaseItem(); }, Define.UIEvent.Click);

        DataInit();
    }

    // ���� �����͵� ScriptableObject�� �����ұ� ��� ��
    private void DataInit()
    {
        string itemName = "", itemInfo = ""; //itemPrice = "";

        switch (_goName)
        {
            case "Reincarnation":
                itemName = "ȯ��";
                itemInfo = "���� �ɷ��� ���� ����� ä�� \n�ٽ� �¾�ϴ�.";
                break;
            case "GiveUp":
                itemName = "����";
                itemInfo = "���� �ɷ� �״�� �ٽ� \n�¾�ϴ�.";
                break;
            case "AddCoin":
                itemName = "���� ����";
                itemInfo = $"���� ��û�ϰ� \n{_addCoin} ���θ� ����ϴ�.";
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
        // ���� �߰� ����
        if (Managers.Data.MyPlayerInfo.Round < 100)
            return;

        Managers.Data.DoReincarnation(true);
    }

    private void GiveUp()
    {
        // ����

        Managers.Data.DoReincarnation(false);
    }

    // �Ϸ� Ƚ�� ���� �ʿ�
    private void AddCoin()
    {
        // ����

        Managers.Game.MyPlayer.StatInfo.Coin += _addCoin;
    }
}
