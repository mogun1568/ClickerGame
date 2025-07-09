using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObject/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    [Header("# Public Info")]
    public Define.ShopItemType shopItemType;
    public int shopItemId;
    public string shopItemKind;
    public string shopItemIcon;
    public string shopItemName;
    public string shopItemInfo;
    public int shopItemPrice;
    public bool isAd;

    [Header("# Skin Info")]
    public Define.ClassType classType;
    public float spawnPosY;
    public float attackAnimTime;

    public ShopItemData Copy()
    {
        ShopItemData copied = CreateInstance<ShopItemData>();
        copied.shopItemType = this.shopItemType;
        copied.shopItemId = this.shopItemId;
        copied.shopItemKind = this.shopItemKind;
        copied.shopItemIcon = this.shopItemIcon;
        copied.shopItemName = this.shopItemName;
        copied.shopItemInfo = this.shopItemInfo;
        copied.shopItemPrice = this.shopItemPrice;
        copied.isAd = this.isAd;

        if (copied.shopItemType == Define.ShopItemType.Skin)
        {
            copied.classType = this.classType;
            copied.spawnPosY = this.spawnPosY;
            copied.attackAnimTime = this.attackAnimTime;
        }

        return copied;
    }
}
