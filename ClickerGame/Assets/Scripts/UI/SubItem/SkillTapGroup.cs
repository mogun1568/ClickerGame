using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTapGroup : UI_Base
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
        ClassSkill,
        CommonSkill
    }

    private GameObject _classAlert, _commomAlert;
    bool _isClass, _isCommon;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetObject((int)GameObjects.CommonSkill).SetActive(false);
    }

    public override void Init()
    {
        Managers.Skill.OnSkillAcquired -= OnSkillAcquired;
        Managers.Skill.OnSkillAcquired += OnSkillAcquired;

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        _classAlert = GetObject((int)GameObjects.Button_Class).transform.GetChild(1).gameObject;
        _commomAlert = GetObject((int)GameObjects.Button_Common).transform.GetChild(1).gameObject;

        BindEvent(GetButton((int)Buttons.Button_Class).gameObject, (PointerEventData data) => { SelectTap("Class"); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Common).gameObject, (PointerEventData data) => { SelectTap("Common"); }, Define.UIEvent.Click);

        _classAlert.SetActive(false);
        _commomAlert.SetActive(false); 
    }

    public void SelectTap(string TapName)
    {
        if (TapName == "Class")
        {
            if (_classAlert.activeSelf) _isClass = true;

            GetObject((int)GameObjects.CommonSkill).SetActive(false);
            GetObject((int)GameObjects.ClassSkill).SetActive(true);
        }
        else
        {
            if (_commomAlert.activeSelf) _isCommon = true;

            GetObject((int)GameObjects.ClassSkill).SetActive(false);
            GetObject((int)GameObjects.CommonSkill).SetActive(true);
        }

        if (_isClass)
        {
            _isClass = false;
            _classAlert.SetActive(false);
        }
        if (_isCommon)
        {
            _isCommon = false;
            _commomAlert.SetActive(false);
        }
    }

    private void OnSkillAcquired(string skillKind)
    {
        // Init에서 다 SetActive(false) 시켜서 처음 킬 때는 안 뜸
        // 임시
        if (GetObject((int)GameObjects.ClassSkill).activeSelf)
            _isClass = true;
        else
            _isCommon = false;

        if (skillKind == "Class")
            _classAlert.SetActive(true);
        else
            _commomAlert.SetActive(true);
    }

    private void OnDisable()
    {
        _classAlert.SetActive(false);
        _commomAlert.SetActive(false);
    }
}
