using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillManager
{
    private List<string> AllSkills;
    private Dictionary<string, float> SkillCoolTime = new(); // 스킬 쿨타임 관리용
    private Dictionary<string, Data.SkillInfo> MyPlayerSkillDict;
    private float _skillDropChance;

    public void Init()
    {
        AllSkills = Managers.Resource.SkillDict.Keys.ToList();
        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
        foreach (var skill in MyPlayerSkillDict)
            SkillCoolTime[skill.Key] = -999f;

        _skillDropChance = 0.05f;
    }

    public void RandomAddSkill()
    {
        if (Random.value > _skillDropChance)
            return;

        Managers.Sound.Play("SFX_Chest_Open_1", Define.Sound.SFX);
        AddSkill();
    }
    
    private void AddSkill()
    {
        int idx = Random.Range(0, AllSkills.Count);
        string randomSkill = AllSkills[idx];

        //Debug.Log($"Drop {randomSkill}");

        if (MyPlayerSkillDict.ContainsKey(randomSkill)) {
            MyPlayerSkillDict[randomSkill].skillLevel++;
            MyPlayerSkillDict[randomSkill].skillValue
                += Managers.Resource.SkillDict[randomSkill].abilityIncreaseValue;
            MyPlayerSkillDict[randomSkill].skillValue
                = Mathf.Round(MyPlayerSkillDict[randomSkill].skillValue * 100f) / 100f;
        }
        else
        {
            MyPlayerSkillDict.Add(randomSkill, CreateDefaultSkillData(randomSkill));
            SkillCoolTime[randomSkill] = -999f;
        }

        // 최대 레벨 스킬 제거, 환생하면 스킬도 초기화할 지 고민 중
        if (MyPlayerSkillDict[randomSkill].skillLevel >= Managers.Resource.SkillDict[randomSkill].abilityMaxLevel)
            AllSkills.Remove(randomSkill);

        Managers.Alert.InvokeAlert(randomSkill);

        if (Managers.Data.GameDataReady)
            Managers.Data.UpdateSKill(randomSkill);
    }

    public Data.SkillInfo CreateDefaultSkillData(string skillKind)
    {
        return new Data.SkillInfo
        {
            skillLevel = Managers.Resource.SkillDict[skillKind].abilityLevel,
            skillValue = Managers.Resource.SkillDict[skillKind].abilityValue
        };
    }

    public string ChooseSkill()
    {
        List<string> availableSkills = new();

        float currentTime = Time.time;

        foreach (var pair in MyPlayerSkillDict)
        {
            string skillKind = pair.Key;
            float coolTime = Managers.Resource.SkillDict[skillKind].skillCoolTime;

            if (currentTime - SkillCoolTime[skillKind] >= coolTime)
                availableSkills.Add(skillKind);
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
