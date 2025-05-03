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
                Coin = 10000,
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

                //{ "MaxHP", new Data.StatInfo { statType = "MaxHP", statIcon = "MaxHPIcon", statLevel = 1, statName = "�ִ� ü��",
                //    statValue = 100.0f, statIncreaseValue = 10.0f, statPrice = 1, statIncreasePrice = 1 }},
                //{ "Regeneration", new Data.StatInfo { statType = "Regeneration", statIcon = "RegenerationIcon", statLevel = 1, statName = "�ڿ� ȸ��",
                //    statValue = 5.0f, statIncreaseValue = 1.0f, statPrice = 10, statIncreasePrice = 10 }},
                //{ "ATK", new Data.StatInfo { statType = "ATK", statIcon = "ATKIcon", statLevel = 1, statName = "���ݷ�",
                //    statValue = 10.0f, statIncreaseValue = 0.5f, statPrice = 1, statIncreasePrice = 2 }},
                //{ "DEF", new Data.StatInfo { statType = "DEF", statIcon = "DEFIcon", statLevel = 1, statName = "����",
                //    statValue = 1.0f, statIncreaseValue = 0.5f, statPrice = 10, statIncreasePrice = 10 }},
                //{ "AttackSpeed", new Data.StatInfo { statType = "AttackSpeed", statIcon = "AttackSpeedIcon", statLevel = 1, statName = "���� �ӵ�",
                //    statValue = 1.0f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }},
                //{ "Range", new Data.StatInfo { statType = "Range", statIcon = "RangeIcon", statLevel = 1, statName = "���� ����",
                //    statValue = 1.5f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }}
            },
            skills = new Dictionary<string, Data.SkillInfo>
            {
            },
            enemys = new Dictionary<string, Data.EnemyInfo>
            {
                { "HeavyBandit", new Data.EnemyInfo { enemyType = "HeavyBandit", enemyName = "HeavyBandit",
                    enemyMaxHP = 30.0f, enemyATK = 10.0f, enemyDEF = 0.0f, enemyAttackSpeed = 1.0f, enemyRange = 1.5f, enemyMoveSpeed = 2.5f, enemyCoin = 10 }}
            }
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
