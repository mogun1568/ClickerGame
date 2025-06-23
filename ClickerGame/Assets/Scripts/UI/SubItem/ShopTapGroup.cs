using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopTapGroup : UI_Base
{
    enum Buttons
    {
        Button_Common,
        Button_Skin,
        Button_Class,
    }

    enum GameObjects
    {
        Button_Common,
        Button_Skin,
        Button_Class,
        CommonShop,
        SkinShop,
        ClassShop,
    }

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.Button_Common).gameObject, (PointerEventData data) => { SelectTap("Common"); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Skin).gameObject, (PointerEventData data) => { SelectTap("Skin"); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Class).gameObject, (PointerEventData data) => { SelectTap("Class"); }, Define.UIEvent.Click);

        ItemInit();

        GetObject((int)GameObjects.SkinShop).SetActive(false);
        GetObject((int)GameObjects.ClassShop).SetActive(false);
    }

    private void ItemInit()
    {
        string purchaseType;
        foreach (var pair in Managers.Resource.CommonItemDict)
        {
            if (pair.Value.isAd) purchaseType = "AdShopItem";
            else purchaseType = "ShopItem";

            GameObject item = Managers.Resource.Instantiate($"UI/SubItem/{purchaseType}", default, GetObject((int)GameObjects.CommonShop).transform.GetChild(0));
            item.name = pair.Key;
            item.GetOrAddComponent<UI_CommonItem>();
        }
        foreach (var pair in Managers.Resource.SkinItemDict)
        {
            if (pair.Value.isAd) purchaseType = "AdShopItem";
            else purchaseType = "ShopItem";

            GameObject item = Managers.Resource.Instantiate($"UI/SubItem/{purchaseType}", default, GetObject((int)GameObjects.SkinShop).transform.GetChild(0));
            item.name = pair.Key;
            item.GetOrAddComponent<UI_SkinItem>();
        }
    }

    public void SelectTap(string TapName)
    {
        if (TapName == "Common")
        {
            GetObject((int)GameObjects.CommonShop).SetActive(true);
            GetObject((int)GameObjects.SkinShop).SetActive(false);
            GetObject((int)GameObjects.ClassShop).SetActive(false);

        }
        else if (TapName == "Skin")
        {
            GetObject((int)GameObjects.CommonShop).SetActive(false);
            GetObject((int)GameObjects.SkinShop).SetActive(true);
            GetObject((int)GameObjects.ClassShop).SetActive(false);
        }
        else
        {
            GetObject((int)GameObjects.CommonShop).SetActive(false);
            GetObject((int)GameObjects.SkinShop).SetActive(false);
            GetObject((int)GameObjects.ClassShop).SetActive(true);
        }
    }
}
