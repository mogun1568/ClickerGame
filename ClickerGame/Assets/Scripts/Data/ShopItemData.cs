using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObject/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    public Define.ShopItemType shopItemType;
    public string shopItemKind;
    public string shopItemIcon;
    public string shopItemName;
    public string shopItemInfo;
    public int shopItemPrice;

    public ShopItemData Copy()
    {
        ShopItemData copied = CreateInstance<ShopItemData>();
        copied.shopItemType = this.shopItemType;
        copied.shopItemKind = this.shopItemKind;
        copied.shopItemIcon = this.shopItemIcon;
        copied.shopItemName = this.shopItemName;
        copied.shopItemInfo = this.shopItemInfo;
        copied.shopItemPrice = this.shopItemPrice;

        return copied;
    }
}
