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
        return new Data.GameData
        {
            info = new Data.Info
            {
                Coin = 10000,
                HP = 100.0f,
                LastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                OfflineReward = 0
            },
            stats = new Dictionary<string, Data.Stat>
            {
                { "MaxHP", new Data.Stat { statType = "MaxHP", statIcon = "HPIcon", statLevel = 1, statName = "�ִ� ü��",
                    statValue = 100.0f, statIncreaseValue = 10.0f, statPrice = 1, statIncreasePrice = 1 }},
                { "Regeneration", new Data.Stat { statType = "Regeneration", statIcon = "RegenerationIcon", statLevel = 1, statName = "�ڿ� ȸ��",
                    statValue = 5.0f, statIncreaseValue = 1.0f, statPrice = 10, statIncreasePrice = 10 }},
                { "ATK", new Data.Stat { statType = "ATK", statIcon = "ATKIcon", statLevel = 1, statName = "���ݷ�",
                    statValue = 10.0f, statIncreaseValue = 0.5f, statPrice = 1, statIncreasePrice = 2 }},
                { "DEF", new Data.Stat { statType = "DEF", statIcon = "DEFIcon", statLevel = 1, statName = "����",
                    statValue = 1.0f, statIncreaseValue = 0.5f, statPrice = 10, statIncreasePrice = 10 }},
                { "AttackSpeed", new Data.Stat { statType = "AttackSpeed", statIcon = "AttackSpeedIcon", statLevel = 1, statName = "���� �ӵ�",
                    statValue = 1.0f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }},
                { "Range", new Data.Stat { statType = "Range", statIcon = "RangeIcon", statLevel = 1, statName = "���� ����",
                    statValue = 1.5f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }}
            },
            skills = new Dictionary<string, Data.Skill>
            {
                { "Knockback", new Data.Skill { skillType = "Knockback", skillIcon = "KnockbackIcon", skillLevel = 0, skillName = "�˹�",
                    skillValue = 1.0f, skillIncreaseValue = 0.2f }},
                { "Slow", new Data.Skill { skillType = "Slow", skillIcon = "SlowIcon", skillLevel = 0, skillName = "���ο�",
                    skillValue = 0.8f, skillIncreaseValue = 0.1f }}

            },
            enemys = new Dictionary<string, Data.Enemy>
            {
                { "HeavyBandit", new Data.Enemy { enemyType = "HeavyBandit", enemyName = "HeavyBandit",
                    enemyMaxHP = 30.0f, enemyATK = 10.0f, enemyDEF = 0.0f, enemyAttackSpeed = 1.0f, enemyRange = 1.5f, enemyMoveSpeed = 2.5f, enemyCoin = 10 }}
            }
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
