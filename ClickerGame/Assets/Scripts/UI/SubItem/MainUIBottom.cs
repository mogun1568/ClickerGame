using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUIBottom : UI_Base
{
    enum Objects
    {
        TapMenu_Stat,
        TapMenu_Skill,
        TapMenu_Inventory,
        TapMenu_Shop,
        Tap_Stat,
        Tap_Skill,
        Tap_Inventory,
        Tap_Shop
    }

    // 버튼 옵젝용 스크립트 따로 만들어야 함
    public GameObject curTapMenu, curTap;

    private void Awake()
    {
        Bind<Object>(typeof(Objects));

        curTapMenu = GetObject((int)Objects.TapMenu_Stat);
        curTap = GetObject((int)Objects.Tap_Stat);
    }

    public void SelectTap(string TapName)
    {
        switch (TapName)
        {
            case "TapMenu_Stat":
                break;
            case "TapMenu_Skill":
                break;
            case "TapMenu_Inventory":
                break;
            case "TapMenu_Shop":
                break;
        }
    }

    public override void Init()
    {
        
    }
}
