using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObject/AbilityData")]
public class AbilityData : ScriptableObject
{
    public enum Type { Stat, Skill }

    [Header("# Public Info")]
    public Define.AbilityType creatureType;
    public string abilityKind;
    public string abilityIcon;
    public string abilityName;
    public int abilityLevel;
    public float abilityValue;
    public float abilityIncreaseValue;

    [Header("# Stat Info")]
    public int statPrice;
    public int statIncreasePrice;

    [Header("# SKill Info")]
    public float skillCoolTime;

    public AbilityData Copy(Define.AbilityType type)
    {
        AbilityData copied = CreateInstance<AbilityData>();
        copied.creatureType = this.creatureType;
        copied.abilityKind = this.abilityKind;
        copied.abilityIcon = this.abilityIcon;
        copied.abilityName = this.abilityName;
        copied.abilityLevel = this.abilityLevel;
        copied.abilityValue = this.abilityValue;
        copied.abilityIncreaseValue = this.abilityIncreaseValue;

        if (type == Define.AbilityType.Stat)
        {
            copied.statPrice = this.statPrice;
            copied.statIncreasePrice = this.statIncreasePrice;
        }
        else
            copied.skillCoolTime = this.skillCoolTime;

        return copied;
    }
}