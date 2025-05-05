using UnityEngine;

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
    private GameObject _alert;
    private bool _chooseSkillTap;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Managers.Skill.OnSkillAcquired -= OnSkillAcquired;
        Managers.Skill.OnSkillAcquired += OnSkillAcquired;

        Bind<GameObject>(typeof(GameObjects));

        _curTapMenu = GetObject((int)GameObjects.TapMenu_Stat).GetComponent<TapUI>();
        _curTap = GetObject((int)GameObjects.Tap_Stat);
        _alert = GetObject((int)GameObjects.TapMenu_Skill).transform.GetChild(1).gameObject;

        _alert.SetActive(false);
        GetObject((int)GameObjects.Tap_Skill).SetActive(false);
        GetObject((int)GameObjects.Tap_Inventory).SetActive(false);
        GetObject((int)GameObjects.Tap_Shop).SetActive(false);
    }

    public void SelectTap(string TapName)
    {
        _curTapMenu.CloseTap();
        _curTap.SetActive(false);

        if (_chooseSkillTap && TapName != "TapMenu_Skill")
        {
            _chooseSkillTap = false;
            _alert.SetActive(false);
        }

        switch (TapName)
        {
            case "TapMenu_Stat":
                _curTap = GetObject((int)GameObjects.Tap_Stat);
                break;
            case "TapMenu_Skill":
                _curTap = GetObject((int)GameObjects.Tap_Skill);
                _chooseSkillTap = true;
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

    private void OnSkillAcquired(string skillKind)
    {
        _alert.SetActive(true);
    }
}
