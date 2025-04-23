using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<SkillData> AllSkills;
    private Dictionary<string, float> SkillCoolTime = new(); // 스킬 쿨타임 관리용
    private Dictionary<string, Data.Skill> MyPlayerSkillDict;

    public void Init()
    {
        AllSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skill"));

        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
        foreach (var skill in MyPlayerSkillDict)
            SkillCoolTime[skill.Key] = -999f;
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
            SkillCoolTime[randomSkill.skillType] = -999f;
        }
    }

    public string ChooseSkill()
    {
        List<string> availableSkills = new();

        float currentTime = Time.time;

        foreach (var pair in MyPlayerSkillDict)
        {
            string skillName = pair.Key;
            float coolTime = pair.Value.skillCoolTime;

            if (currentTime - SkillCoolTime[skillName] >= coolTime)
                availableSkills.Add(skillName);
        }

        if (availableSkills.Count == 0)
            return null;

        int randIndex = Random.Range(0, availableSkills.Count);
        return availableSkills[randIndex];
    }

}
