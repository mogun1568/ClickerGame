using UnityEngine;

[CreateAssetMenu(fileName = "SkinItem", menuName = "ScriptableObject/SkinItemData")]
public class SkinItemData : ScriptableObject
{
    public Define.ClassType classType;
    public string skinName;
    public float SpawnPosY;

    public SkinItemData Copy()
    {
        SkinItemData copied = CreateInstance<SkinItemData>();
        copied.classType = this.classType;
        copied.skinName = this.skinName;
        copied.SpawnPosY = this.SpawnPosY;

        return copied;
    }
}