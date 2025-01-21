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
    public Dictionary<string, Data.Enemy> EnemyStatDict { get; private set; } = new Dictionary<string, Data.Enemy>();

    public void Init()
    {
        MyPlayerStatDict = LoadJson<Data.StatData, string, Data.Stat>("PlayerStatData").MakeDict();
        //EnemyStatDict = LoadJson<Data.EnemyData, string, Data.Enemy>("EnemyStatData").MakeDict();
    }

    // 이 부분 잘 모르겠음
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    // 수정 필요
    public void SaveJson<Loader, Key, Value>(Loader data, string path) where Loader : ILoader<Key, Value>
    {
        // 객체를 JSON 문자열로 변환
        string jsonString = JsonUtility.ToJson(data, true);

        // 저장할 파일 경로 설정
        //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
        string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

        // JSON 파일로 저장
        File.WriteAllText(filePath, jsonString);

        Debug.Log($"JSON 저장 완료: {filePath}");
    }
}
