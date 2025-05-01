using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    // �׻� ����ȭ�� ���������� �ǰ� �ϱ� ���� public + Serializable �߰�
    [SerializeField]
    public Data.SkillInfo skillData = new Data.SkillInfo();
}