using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    // 항상 직렬화가 정상적으로 되게 하기 위해 public + Serializable 추가
    [SerializeField]
    public Data.SkillInfo skillData = new Data.SkillInfo();
}