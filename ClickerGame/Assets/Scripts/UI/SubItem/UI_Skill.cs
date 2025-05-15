using Cysharp.Threading.Tasks;
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
        Text_SkillValue
    }

    enum GameObjects
    {
        Alert_s_Red
    }

    private string _goName;
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

        _goName = gameObject.name;
        _blue = GetComponent<Image>().color;
        _glay = Color.gray;

        GetObject((int)GameObjects.Alert_s_Red).SetActive(false);
    }

    private async UniTask DataInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        //Image icon = GetImage((int)Images.Icon_Skill);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");

        if (Managers.Data.MyPlayerSkillDict.ContainsKey(_goName))
        {
            GetText((int)Texts.Text_SkillName).text = Managers.Resource.SkillDict[_goName].abilityName;
            HUDUpdate();
        }
        else
        {
            GetComponent<Image>().color = _glay;
            GetText((int)Texts.Text_SkillLevel).text = "0";
            GetText((int)Texts.Text_SkillValue).text = "0";
        }
    }

    private void OnSkillAcquired(string skillKind)
    {
        if (skillKind != _goName)
            return;

        HUDUpdate();
    }

    public void HUDUpdate()
    {
        GetText((int)Texts.Text_SkillLevel).text = Managers.Data.MyPlayerSkillDict[_goName].skillLevel.ToString();
        GetText((int)Texts.Text_SkillValue).text = Managers.Data.MyPlayerSkillDict[_goName].skillValue.ToString();
        GetObject((int)GameObjects.Alert_s_Red).SetActive(true);

        if (GetText((int)Texts.Text_SkillLevel).text == "1")
            GetComponent<Image>().color = _blue;
    }

    private void OnDisable()
    {
        GetObject((int)GameObjects.Alert_s_Red).SetActive(false);
    }
}
