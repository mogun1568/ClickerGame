using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : UI_Base
{
    enum Images
    {
        Icon_Skill
    }

    enum Texts
    {
        Text_SkillLevel,
        Text_SkillName,
        Text_SkillValue,
        Text_SkillMaxLevel
    }

    enum GameObjects
    {
        Alert_s_Red
    }

    private Dictionary<string, Data.SkillInfo> _myPlayerSkillDict;
    private Dictionary<string, AbilityData> _skillDict;
    private string _skillName;

    private int _skillMaxlevel;
    private Color _blue, _glay;

    private void Awake()
    {
        Init();
        DataInitAsync().Forget();
    }

    public override void Init()
    {
        Managers.Skill.OnSkillAcquired -= OnSkillAcquired;
        Managers.Skill.OnSkillAcquired += OnSkillAcquired;

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _skillName = gameObject.name;
        _skillDict = Managers.Resource.SkillDict;

        _skillMaxlevel = _skillDict[_skillName].abilityMaxLevel;
        _blue = GetComponent<Image>().color;
        _glay = Color.gray;

        GetObject((int)GameObjects.Alert_s_Red).SetActive(false);

        //Image icon = GetImage((int)Images.Icon_Skill);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");
        GetText((int)Texts.Text_SkillName).text = _skillDict[_skillName].abilityName;
        GetText((int)Texts.Text_SkillLevel).text = "0";
        GetText((int)Texts.Text_SkillValue).text = "0";
        GetText((int)Texts.Text_SkillMaxLevel).text = "최대 레벨\n" + _skillMaxlevel.ToString();

    }

    private async UniTask DataInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        _myPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
        if (_myPlayerSkillDict.ContainsKey(_skillName)) 
            HUDUpdate();
        else
            GetComponent<Image>().color = _glay;
    }

    private void OnSkillAcquired(string skillKind)
    {
        if (skillKind != _skillName)
            return;

        HUDUpdate();
    }

    public void HUDUpdate()
    {
        GetText((int)Texts.Text_SkillLevel).text = _myPlayerSkillDict[_skillName].skillLevel.ToString();
        GetText((int)Texts.Text_SkillValue).text = _myPlayerSkillDict[_skillName].skillValue.ToString();
        GetObject((int)GameObjects.Alert_s_Red).SetActive(true);

        if (GetText((int)Texts.Text_SkillLevel).text == "1")
            GetComponent<Image>().color = _blue;
    }

    private void OnDisable()
    {
        GetObject((int)GameObjects.Alert_s_Red).SetActive(false);
    }
}
