using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, AbilityData> StatDict = new Dictionary<string, AbilityData>();
    public Dictionary<string, AbilityData> SkillDict = new Dictionary<string, AbilityData>();
    public Dictionary<string, EnemyData> EnemyDict = new Dictionary<string, EnemyData>();
    public Dictionary<string, ShopItemData> CommonItemDict = new Dictionary<string, ShopItemData>();
    public Dictionary<string, ShopItemData> SkinItemDict = new Dictionary<string, ShopItemData>();

    public List<AbilityData> StatList = new List<AbilityData>();
    public List<AbilityData> SkillList = new List<AbilityData>();
    public List<ShopItemData> CommonItemList = new List<ShopItemData>();
    public List<ShopItemData> SkinItemList = new List<ShopItemData>();

    public void Init()
    {
        AbilityData[] StatDataAssets = Resources.LoadAll<AbilityData>("Data/StatData");
        AbilityData[] SkillDataAssets = Resources.LoadAll<AbilityData>("Data/SkillData");
        EnemyData[] EnemyDataAssets = Resources.LoadAll<EnemyData>("Data/EnemyData");
        ShopItemData[] CommonItemDataAssets = Resources.LoadAll<ShopItemData>("Data/CommonItemData");
        ShopItemData[] SkinItemDataAssets = Resources.LoadAll<ShopItemData>("Data/SkinItemData");

        foreach (AbilityData data in StatDataAssets)
        {
            AbilityData copy = data.Copy();
            StatDict[copy.abilityKind] = copy;
            StatList.Add(copy);
        }
        foreach (AbilityData data in SkillDataAssets)
        {
            AbilityData copy = data.Copy();
            SkillDict[copy.abilityKind] = copy;
            SkillList.Add(copy);
        }
        foreach (EnemyData data in EnemyDataAssets)
        {
            EnemyData copy = data.Copy();
            EnemyDict[copy.enemyName] = copy;
        }
        foreach (ShopItemData data in CommonItemDataAssets)
        {
            ShopItemData copy = data.Copy();
            CommonItemDict[copy.shopItemKind] = copy;
            CommonItemList.Add(copy);
        }
        foreach (ShopItemData data in SkinItemDataAssets)
        {
            ShopItemData copy = data.Copy();
            SkinItemDict[copy.shopItemKind] = copy;
            SkinItemList.Add(copy);
        }

        StatList = StatList.OrderBy(x => x.abilityId).ToList();
        SkillList = SkillList.OrderBy(x => x.abilityId).ToList();
        CommonItemList = CommonItemList.OrderBy(x => x.shopItemId).ToList();
        SkinItemList = SkinItemList.OrderBy(x => x.shopItemId).ToList();
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
            {
                name = name.Substring(index + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Vector3 position = default, Transform parent = null)
    {
        // 1. original �̹� ��� ������ �ٷ� ���
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Logging.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 2. Ȥ�� Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, position, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, position, Quaternion.identity, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // ���࿡ Ǯ���� �ʿ��� ���̶�� -> Ǯ�� �Ŵ������� ��Ź
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
