using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalDataManager
{
    private string _fileName = "GameData";
    private string _TestFileName = "GameDataTest";
    private string _filePath;

    private Dictionary<string, AbilityData> _statDict;

    public T LoadLocalData<T>()
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");

        // 파일이 존재하면 로드, 없으면 기본값 반환
        if (File.Exists(_filePath))
        {
            string jsonString = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        return default;


        //TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{_fileName}");
        //return JsonConvert.DeserializeObject<T>(textAsset.text);
    }

    public void SaveLocalData<T>(T data)
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");

        // 직렬화 시 Dictionary도 정상적으로 변환됨(이걸 위해 JsonUtility 대신 Newtonsoft 사용)
        // 지금은 알아보기 쉽기 위해 Indented지만 빌드때는 None로 해야 크기 줄일 수 있음
        string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_filePath, jsonString);

        //_filePath = Path.Combine(Application.dataPath, $"Resources/Data/{_TestFileName}.json");
        //// 지금은 알아보기 쉽기 위해 Indented지만 빌드때는 None로 해야 크기 줄일 수 있음
        //string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        //File.WriteAllText(_filePath, jsonString);
    }

    public bool HasData()
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");
        return File.Exists(_filePath);
    }

    // 해킹 방지로 하드 코딩과 암호화 중에 고민 중
    public Data.GameData CreateDefaultGameData()
    {
        _statDict = Managers.Resource.StatDict;

        return new Data.GameData
        {
            info = new Data.Info
            {
                Nickname = "Guest",
                Reincarnation = 0,
                Coin = 10000,
                Round = 1,
                Map = "Plain",
                Class = "Knight",
                Skin = "Knight",
                HP = 100.0f,
                LastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                OfflineReward = 0
            },
            stats = new Dictionary<string, Data.StatInfo>
            {
                { "MaxHP", CreateDefaultStatData("MaxHP") },
                { "Regeneration", CreateDefaultStatData("Regeneration") },
                { "ATK", CreateDefaultStatData("ATK") },
                { "DEF", CreateDefaultStatData("DEF") },
                { "AttackSpeed", CreateDefaultStatData("AttackSpeed") },
                { "Range", CreateDefaultStatData("Range") }
            },
            skills = new Dictionary<string, Data.SkillInfo> { },
            skins = new List<string>()
        };
    }

    public Data.StatInfo CreateDefaultStatData(string statKind)
    {
        return new Data.StatInfo
        {
            statLevel = _statDict[statKind].abilityLevel,
            statValue = _statDict[statKind].abilityValue,
            statPrice = _statDict[statKind].statPrice,
        };
    }

    public void DeleteData()
    {
        // 로컬 데이터가 저장된 파일 경로
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");

        // 파일이 존재하면 삭제
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
            Debug.Log("Local data has been deleted.");
        }
        else
        {
            Debug.LogWarning("No local data found to delete.");
        }
    }
}
