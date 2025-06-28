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

        // ������ �����ϸ� �ε�, ������ �⺻�� ��ȯ
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

        // ����ȭ �� Dictionary�� ���������� ��ȯ��(�̰� ���� JsonUtility ��� Newtonsoft ���)
        // ������ �˾ƺ��� ���� ���� Indented���� ���嶧�� None�� �ؾ� ũ�� ���� �� ����
        string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_filePath, jsonString);

        //_filePath = Path.Combine(Application.dataPath, $"Resources/Data/{_TestFileName}.json");
        //// ������ �˾ƺ��� ���� ���� Indented���� ���嶧�� None�� �ؾ� ũ�� ���� �� ����
        //string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        //File.WriteAllText(_filePath, jsonString);
    }

    public bool HasData()
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");
        return File.Exists(_filePath);
    }

    // ��ŷ ������ �ϵ� �ڵ��� ��ȣȭ �߿� ��� ��
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
        // ���� �����Ͱ� ����� ���� ���
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");

        // ������ �����ϸ� ����
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
