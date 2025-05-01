using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<Data.SkillInfo> AllSkills;
    private Dictionary<string, float> SkillCoolTime = new(); // 스킬 쿨타임 관리용
    private Dictionary<string, Data.SkillInfo> MyPlayerSkillDict;
    private float _skillDropChance;

    public void Init()
    {
        SkillData[] skillDataAssets = Resources.LoadAll<SkillData>("Data/SkillData");

        AllSkills = new List<Data.SkillInfo>();

        foreach (SkillData skillData in skillDataAssets)
        {
            // 복사해서 새로운 SkillInfo로 저장
            Data.SkillInfo copied = new Data.SkillInfo(skillData.skillData);
            AllSkills.Add(copied);
        }

        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
        foreach (var skill in MyPlayerSkillDict)
            SkillCoolTime[skill.Key] = -999f;

        _skillDropChance = 0.1f;
    }

    public void RandomAddSkill()
    {
        if (Random.value > _skillDropChance)
            return;

        AddSkill();
    }
    
    private void AddSkill()
    {
        int idx = Random.Range(0, AllSkills.Count);
    
        Data.SkillInfo randomSkill = AllSkills[idx];

        Debug.Log($"Drop {randomSkill.skillType}");

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
        {
            //Debug.Log("All Skills CoolTime.");
            return null;
        }
            
        int randIndex = Random.Range(0, availableSkills.Count);

        SkillCoolTime[availableSkills[randIndex]] = currentTime;
        return availableSkills[randIndex];
    }

}
