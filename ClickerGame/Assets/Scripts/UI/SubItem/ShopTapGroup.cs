using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopTapGroup : UI_Base
{
    enum Buttons
    {
        Button_Class,
        Button_Common
    }

    enum GameObjects
    {
        Button_Class,
        Button_Common,
        ClassShop,
        CommonShop
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.Button_Class).gameObject, (PointerEventData data) => { SelectTap("Class"); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Common).gameObject, (PointerEventData data) => { SelectTap("Common"); }, Define.UIEvent.Click);

        GetObject((int)GameObjects.CommonShop).SetActive(false);
    }

    public void SelectTap(string TapName)
    {
        if (TapName == "Class")
        {
            GetObject((int)GameObjects.CommonShop).SetActive(false);
            GetObject((int)GameObjects.ClassShop).SetActive(true);
        }
        else
        {
            GetObject((int)GameObjects.ClassShop).SetActive(false);
            GetObject((int)GameObjects.CommonShop).SetActive(true);
        }
    }
}
