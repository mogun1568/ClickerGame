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
    
    private string _path;

    public void Init()
    {
        MyPlayerStatDict = LoadJson<Data.StatData, string, Data.Stat>("MyPlayerStatData").MakeDict();
        EnemyDict = LoadJson<Data.EnemyData, string, Data.Enemy>("EnemyData").MakeDict();
        _path = Application.persistentDataPath;
    }

    // �� �κ� �� �𸣰���
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //string textAsset = File.ReadAllText(Path.Combine(_path, $"{path}.json"));
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    // SaveJson<Loader, Key, Value>(Loader data, string path)
    public void SaveJson<T>(T data, string path)
    {
        // ��ü�� JSON ���ڿ��� ��ȯ
        string jsonString = JsonUtility.ToJson(data, true);

        // ������ ���� ��� ����
        //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
        string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

        // JSON ���Ϸ� ����
        File.WriteAllText(filePath, jsonString);

        //Debug.Log($"JSON ���� �Ϸ�: {filePath}");
    }
}
