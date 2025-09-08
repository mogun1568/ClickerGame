using UnityEngine;

public class TabGroup : UI_Base
{
    enum GameObjects
    {
        TabMenu_Stat,
        TabMenu_Skill,
        TabMenu_Shop,
        TabMenu_Ranking,
        Tab_Stat,
        Tab_Skill,
        Tab_Shop,
        Tab_Ranking
    }

    [HideInInspector]
    public TabUI _curTabMenu;
    private GameObject _curTab;
    private GameObject _alert;
    private bool _chooseSkillTab;

    void Awake()
    {
        Init();
    }

    void Start()
    {
        GetObject((int)GameObjects.Tab_Skill).SetActive(false);
        GetObject((int)GameObjects.Tab_Shop).SetActive(false);
        GetObject((int)GameObjects.Tab_Ranking).SetActive(false);
    }

    public override void Init()
    {
        Managers.Alert.OnAlertAcquired -= OnSkillAcquired;
        Managers.Alert.OnAlertAcquired += OnSkillAcquired;

        Bind<GameObject>(typeof(GameObjects));

        _curTabMenu = GetObject((int)GameObjects.TabMenu_Stat).GetComponent<TabUI>();
        _curTab = GetObject((int)GameObjects.Tab_Stat);
        _alert = GetObject((int)GameObjects.TabMenu_Skill).transform.GetChild(1).gameObject;

        _alert.SetActive(false); 
    }

    public void SelectTab(string TabName)
    {
        _curTabMenu.CloseTab();
        _curTab.SetActive(false);

        switch (TabName)
        {
            case "TabMenu_Stat":
                _curTab = GetObject((int)GameObjects.Tab_Stat);
                break;
            case "TabMenu_Skill":
                _curTab = GetObject((int)GameObjects.Tab_Skill);
                _chooseSkillTab = true;
                break;
            case "TabMenu_Shop":
                _curTab = GetObject((int)GameObjects.Tab_Shop);
                break;
            case "TabMenu_Ranking":
                _curTab = GetObject((int)GameObjects.Tab_Ranking);
                break;
        }
        _curTab.SetActive(true);

        if (_chooseSkillTab)
        {
            _chooseSkillTab = false;
            _alert.SetActive(false);
        }
    }

    private void OnSkillAcquired(string skillKind)
    {
        if (_curTab.name == "Tab_Skill")
            _chooseSkillTab = true;

        _alert.SetActive(true);
    }
}
