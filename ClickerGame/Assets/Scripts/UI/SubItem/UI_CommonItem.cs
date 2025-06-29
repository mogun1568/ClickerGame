using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CommonItem : UI_Base
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
        Text_Purchase
    }

    private string _goName;

    private string _reincarnationText = "라운드 100 이상";
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
        Dictionary<string, ShopItemData> commonItemDict = Managers.Resource.CommonItemDict;

        Image icon = GetImage((int)Images.Icon_Item);
        icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{commonItemDict[_goName].shopItemIcon}");
        GetText((int)Texts.Text_ItemName).text = commonItemDict[_goName].shopItemName;

        switch (_goName)
        {
            case "AddCoin":
                GetText((int)Texts.Text_ItemInfo).text = _addCoin.ToString() + commonItemDict[_goName].shopItemInfo;
                break;
            default:
                GetText((int)Texts.Text_ItemInfo).text = commonItemDict[_goName].shopItemInfo;
                break;
        }

        if (commonItemDict[_goName].shopItemPrice != 0)
            GetText((int)Texts.Text_Purchase).text = $"구매\n{commonItemDict[_goName].shopItemPrice.ToString()}";
        else
            GetText((int)Texts.Text_Purchase).text = "구매";
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
        if (Managers.Data.MyPlayerInfo.Round < 100)
        {
            Managers.UI._toastMessage.Show(_reincarnationText);
            return;
        }   

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
