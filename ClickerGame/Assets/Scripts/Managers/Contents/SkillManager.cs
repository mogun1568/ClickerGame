using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    public List<SkillData> AllSkills;
    public List<(string name, int coolTime)> SkillList = new List<(string, int)>();
    private Dictionary<string, Data.Skill> MyPlayerSkillDict;

    public void Init()
    {
        AllSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skill"));

        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
        foreach (var skill in MyPlayerSkillDict)
            SkillList.Add((skill.Value.skillType, skill.Value.skillCoolTime));
    }

    public void AddSkill()
    {
        int idx = Random.Range(0, AllSkills.Count);
        Data.Skill randomSkill = AllSkills[idx].skillData;

        if (MyPlayerSkillDict.ContainsKey(randomSkill.skillType)) {
            MyPlayerSkillDict[randomSkill.skillType].skillLevel++;
            MyPlayerSkillDict[randomSkill.skillType].skillValue
                += MyPlayerSkillDict[randomSkill.skillType].skillIncreaseValue;
        }
        else
        {
            MyPlayerSkillDict.Add(randomSkill.skillType, randomSkill);
            SkillList.Add((randomSkill.skillType, randomSkill.skillCoolTime));
        }
    }

    public string ChooseSkill()
    {
        if (SkillList == null || SkillList.Count == 0)
        {
            Debug.Log("skillList is null.");
            return null;
        }

        // 스킬 선택

        string name = "";
        return name;
    }
}
