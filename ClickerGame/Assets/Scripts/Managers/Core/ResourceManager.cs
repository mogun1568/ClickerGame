using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, AbilityData> StatDict = new Dictionary<string, AbilityData>();
    public Dictionary<string, AbilityData> SkillDict = new Dictionary<string, AbilityData>();
    public Dictionary<string, EnemyData> EnemyDict = new Dictionary<string, EnemyData>();
    public Dictionary<string, ShopItemData> ShopItemDict = new Dictionary<string, ShopItemData>();

    public void Init()
    {
        AbilityData[] StatDataAssets = Resources.LoadAll<AbilityData>("Data/StatData");
        AbilityData[] SkillDataAssets = Resources.LoadAll<AbilityData>("Data/SkillData");
        EnemyData[] EnemyDataAssets = Resources.LoadAll<EnemyData>("Data/EnemyData");
        ShopItemData[] ShopItemDataAssets = Resources.LoadAll<ShopItemData>("Data/ShopItemData");

        foreach (AbilityData data in StatDataAssets)
        {
            AbilityData copy = data.Copy(Define.AbilityType.Stat);
            StatDict[copy.abilityKind] = copy;
        }
        foreach (AbilityData data in SkillDataAssets)
        {
            AbilityData copy = data.Copy(Define.AbilityType.Skill);
            SkillDict[copy.abilityKind] = copy;
        }
        foreach (EnemyData data in EnemyDataAssets)
        {
            EnemyData copy = data.Copy();
            EnemyDict[copy.enemyName] = copy;
        }
        foreach (ShopItemData data in ShopItemDataAssets)
        {
            ShopItemData copy = data.Copy();
            ShopItemDict[copy.shopItemKind] = copy;
        }
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
        // 1. original 이미 들고 있으면 바로 사용
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 2. 혹시 풀링된 애가 있을까?
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

        // 만약에 풀링이 필요한 아이라면 -> 풀링 매니저한테 위탁
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
