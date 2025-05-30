using UnityEngine;

public class TapGroup : UI_Base
{
    enum GameObjects
    {
        TapMenu_Stat,
        TapMenu_Skill,
        TapMenu_Shop,
        TapMenu_Ranking,
        Tap_Stat,
        Tap_Skill,
        Tap_Shop,
        Tap_Ranking
    }

    [HideInInspector]
    public TapUI _curTapMenu;
    private GameObject _curTap;
    private GameObject _alert;
    private bool _chooseSkillTap;

    void Awake()
    {
        Init();
    }

    void Start()
    {
        GetObject((int)GameObjects.Tap_Skill).SetActive(false);
        GetObject((int)GameObjects.Tap_Shop).SetActive(false);
        GetObject((int)GameObjects.Tap_Ranking).SetActive(false);
    }

    public override void Init()
    {
        Managers.Alert.OnAlertAcquired -= OnSkillAcquired;
        Managers.Alert.OnAlertAcquired += OnSkillAcquired;

        Bind<GameObject>(typeof(GameObjects));

        _curTapMenu = GetObject((int)GameObjects.TapMenu_Stat).GetComponent<TapUI>();
        _curTap = GetObject((int)GameObjects.Tap_Stat);
        _alert = GetObject((int)GameObjects.TapMenu_Skill).transform.GetChild(1).gameObject;

        _alert.SetActive(false); 
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
                _chooseSkillTap = true;
                break;
            case "TapMenu_Shop":
                _curTap = GetObject((int)GameObjects.Tap_Shop);
                break;
            case "TapMenu_Ranking":
                _curTap = GetObject((int)GameObjects.Tap_Ranking);
                break;
        }
        _curTap.SetActive(true);

        if (_chooseSkillTap)
        {
            _chooseSkillTap = false;
            _alert.SetActive(false);
        }
    }

    private void OnSkillAcquired(string skillKind)
    {
        if (_curTap.name == "Tap_Skill")
            _chooseSkillTap = true;

        _alert.SetActive(true);
    }
}
