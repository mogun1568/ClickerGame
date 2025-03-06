using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapGroup : UI_Base
{
    enum GameObjects
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

    [HideInInspector]
    public TapUI _curTapMenu;
    private GameObject _curTap;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        _curTapMenu = GetObject((int)GameObjects.TapMenu_Stat).GetComponent<TapUI>();
        _curTap = GetObject((int)GameObjects.Tap_Stat);

        GetObject((int)GameObjects.TapMenu_Skill).GetComponent<TapUI>();
        GetObject((int)GameObjects.TapMenu_Inventory).GetComponent<TapUI>();
        GetObject((int)GameObjects.TapMenu_Shop).GetComponent<TapUI>();
        GetObject((int)GameObjects.Tap_Skill).SetActive(false);
        GetObject((int)GameObjects.Tap_Inventory).SetActive(false);
        GetObject((int)GameObjects.Tap_Shop).SetActive(false);
    }

    public void SelectTap(string TapName)
    {
        _curTapMenu.CloseTap();
        _curTap.SetActive(false);

        switch (TapName)
        {
            case "TapMenu_Stat":
                _curTap = GetObject((int)GameObjects.Tap_Stat);
                break;
            case "TapMenu_Skill":
                _curTap = GetObject((int)GameObjects.Tap_Skill);
                break;
            case "TapMenu_Inventory":
                _curTap = GetObject((int)GameObjects.Tap_Inventory);
                break;
            case "TapMenu_Shop":
                _curTap = GetObject((int)GameObjects.Tap_Shop);
                break;
        }

        _curTap.SetActive(true);
    }
}
