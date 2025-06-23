using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkinItem : UI_Base
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

        DataInit().Forget();
    }

    // 상점 데이터도 ScriptableObject로 관리할까 고민 중
    private async UniTask DataInit()
    {
        Dictionary<string, ShopItemData> SkinItemDict = Managers.Resource.SkinItemDict;

        Image icon = GetImage((int)Images.Icon_Item);
        icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{SkinItemDict[_goName].shopItemIcon}");

        GetText((int)Texts.Text_ItemName).text = SkinItemDict[_goName].shopItemName;
        GetText((int)Texts.Text_ItemInfo).text = SkinItemDict[_goName].shopItemInfo;

        if (SkinItemDict[_goName].shopItemPrice != 0)
            GetText((int)Texts.Text_ItemPrice).text = SkinItemDict[_goName].shopItemPrice.ToString();

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
        if (_goName == Managers.Data.MyPlayerInfo.Skin)
            GetText((int)Texts.Text_ItemName).text += "(현재 스킨)";
    }

    private void PurchaseItem()
    {
        if (_goName == Managers.Data.MyPlayerInfo.Skin)
            return;

        Managers.Data.MyPlayerInfo.Skin = _goName;
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }
}
