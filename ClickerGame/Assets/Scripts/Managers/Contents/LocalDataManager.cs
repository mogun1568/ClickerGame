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
            return JsonUtility.FromJson<T>(jsonString);
        }
        else
            return default;


        //TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{_fileName}");
        //return JsonUtility.FromJson<T>(textAsset.text);
    }

    public void SaveLocalData<T>(T data)
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{_fileName}.json");
        // ������ �˾ƺ��� ���� True, ���� �Ŀ��� False�� ����
        string jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(_filePath, jsonString);

        //_filePath = Path.Combine(Application.dataPath, $"Resources/Data/{_TestFileName}.json");
        //// ������ �˾ƺ��� ���� ���� true���� ���嶧�� false�� �ؾ� ��
        //string jsonString = JsonUtility.ToJson(data, true);
        //File.WriteAllText(_filePath, jsonString);
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
                LastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            stats = new List<Data.Stat>
            {
                new Data.Stat { statType = "MaxHP", statIcon = "HPIcon", statLevel = 1, statName = "�ִ� ü��", 
                    statValue = 100.0f, statIncreaseValue = 10.0f, statPrice = 1, statIncreasePrice = 1 },
                new Data.Stat { statType = "Regeneration", statIcon = "RegenerationIcon", statLevel = 1, statName = "�ڿ� ȸ��", 
                    statValue = 5.0f, statIncreaseValue = 1.0f, statPrice = 10, statIncreasePrice = 10 },
                new Data.Stat { statType = "ATK", statIcon = "ATKIcon", statLevel = 1, statName = "���ݷ�", 
                    statValue = 10.0f, statIncreaseValue = 0.5f, statPrice = 1, statIncreasePrice = 2 },
                new Data.Stat { statType = "DEF", statIcon = "DEFIcon", statLevel = 1, statName = "����", 
                    statValue = 1.0f, statIncreaseValue = 0.5f, statPrice = 10, statIncreasePrice = 10 },
                new Data.Stat { statType = "AttackSpeed", statIcon = "AttackSpeedIcon", statLevel = 1, statName = "���� �ӵ�", 
                    statValue = 1.0f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 },
                new Data.Stat { statType = "Range", statIcon = "RangeIcon", statLevel = 1, statName = "���� ����", 
                    statValue = 1.5f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }
            },
            enemys = new List<Data.Enemy>
            {
                new Data.Enemy { enemyType = "HeavyBandit", enemyName = "HeavyBandit", 
                    enemyMaxHP = 30.0f, enemyATK = 10.0f, enemyDEF = 0.0f, enemyAttackSpeed = 1.0f, enemyRange = 1.5f, enemyCoin = 10 }
            }
        };
    }
}
