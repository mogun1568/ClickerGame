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
                { "MaxHP", new Data.Stat { statType = "MaxHP", statIcon = "HPIcon", statLevel = 1, statName = "최대 체력",
                    statValue = 100.0f, statIncreaseValue = 10.0f, statPrice = 1, statIncreasePrice = 1 }},
                { "Regeneration", new Data.Stat { statType = "Regeneration", statIcon = "RegenerationIcon", statLevel = 1, statName = "자연 회복",
                    statValue = 5.0f, statIncreaseValue = 1.0f, statPrice = 10, statIncreasePrice = 10 }},
                { "ATK", new Data.Stat { statType = "ATK", statIcon = "ATKIcon", statLevel = 1, statName = "공격력",
                    statValue = 10.0f, statIncreaseValue = 0.5f, statPrice = 1, statIncreasePrice = 2 }},
                { "DEF", new Data.Stat { statType = "DEF", statIcon = "DEFIcon", statLevel = 1, statName = "방어력",
                    statValue = 1.0f, statIncreaseValue = 0.5f, statPrice = 10, statIncreasePrice = 10 }},
                { "AttackSpeed", new Data.Stat { statType = "AttackSpeed", statIcon = "AttackSpeedIcon", statLevel = 1, statName = "공격 속도",
                    statValue = 1.0f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }},
                { "Range", new Data.Stat { statType = "Range", statIcon = "RangeIcon", statLevel = 1, statName = "공격 범위",
                    statValue = 1.5f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 }}
            },
            skills = new Dictionary<string, Data.Skill>
            {
                { "Knockback", new Data.Skill { skillType = "Knockback", skillIcon = "KnockbackIcon", skillLevel = 0, skillName = "넉백",
                    skillValue = 1.0f, skillIncreaseValue = 0.2f }},
                { "Slow", new Data.Skill { skillType = "Slow", skillIcon = "SlowIcon", skillLevel = 0, skillName = "슬로우",
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
