using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        BindEvent(GetButton((int)Buttons.Button_Purchase).gameObject, (PointerEventData data) => { PurchaseItem(); }, Define.UIEvent.Click, false);

        DataInit();
    }

    // 상점 데이터도 ScriptableObject로 관리할까 고민 중
    private void DataInit()
    {
        Dictionary<string, ShopItemData> shopItemDict = Managers.Resource.ShopItemDict;

        Image icon = GetImage((int)Images.Icon_Item);
        icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{shopItemDict[_goName].shopItemIcon}");
        GetText((int)Texts.Text_ItemName).text = shopItemDict[_goName].shopItemName;

        switch (_goName)
        {
            case "AddCoin":
                GetText((int)Texts.Text_ItemInfo).text = _addCoin.ToString() + shopItemDict[_goName].shopItemInfo;
                break;
            default:
                GetText((int)Texts.Text_ItemInfo).text = shopItemDict[_goName].shopItemInfo;
                break;
        }

        if (shopItemDict[_goName].shopItemPrice != 0)
            GetText((int)Texts.Text_ItemPrice).text = shopItemDict[_goName].shopItemPrice.ToString();
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
        Managers.GoogleAd.ShowRewardedAd(Define.RewardAdType.GiveUp, () => { Managers.Data.DoReincarnation(false); });
    }

    // 하루 횟수 제한 필요
    private void AddCoin()
    {
        // 광고
        Managers.GoogleAd.ShowRewardedAd(Define.RewardAdType.AddCoin, () => { Managers.Game.MyPlayer.StatInfo.Coin += _addCoin; });
        Managers.Sound.Play("SFX_Cash_Register_Buy_Click_1", Define.Sound.SFX);
    }
}
