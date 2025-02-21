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

    // 이 부분 잘 모르겠음
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //string textAsset = File.ReadAllText(Path.Combine(_path, $"{path}.json"));
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    // SaveJson<Loader, Key, Value>(Loader data, string path)
    public void SaveJson<T>(T data, string path)
    {
        // 객체를 JSON 문자열로 변환
        string jsonString = JsonUtility.ToJson(data, true);

        // 저장할 파일 경로 설정
        //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
        string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

        // JSON 파일로 저장
        File.WriteAllText(filePath, jsonString);

        //Debug.Log($"JSON 저장 완료: {filePath}");
    }
}
