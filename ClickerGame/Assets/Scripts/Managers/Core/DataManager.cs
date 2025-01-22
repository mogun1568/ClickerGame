using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, Data.Stat> MyPlayerStatDict { get; private set; } = new Dictionary<string, Data.Stat>();
    public Dictionary<string, Data.Enemy> EnemyDict { get; private set; } = new Dictionary<string, Data.Enemy>();

    public void Init()
    {
        MyPlayerStatDict = LoadJson<Data.StatData, string, Data.Stat>("MyPlayerStatData").MakeDict();
        EnemyDict = LoadJson<Data.EnemyData, string, Data.Enemy>("EnemyData").MakeDict();
    }

    // �� �κ� �� �𸣰���
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    // ���� �ʿ�
    public void SaveJson<Loader, Key, Value>(Loader data, string path) where Loader : ILoader<Key, Value>
    {
        // ��ü�� JSON ���ڿ��� ��ȯ
        string jsonString = JsonUtility.ToJson(data, true);

        // ������ ���� ��� ����
        //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
        string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

        // JSON ���Ϸ� ����
        File.WriteAllText(filePath, jsonString);

        Debug.Log($"JSON ���� �Ϸ�: {filePath}");
    }
}
