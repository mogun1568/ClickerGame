using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalDataManager
{
    public T LoadLocalData<T>(string filePath)
    {
        //string jsonString = File.ReadAllText(filePath);
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{filePath}");
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public void SaveLocalData<T>(T data, string filePath)
    {
        filePath = Path.Combine(Application.dataPath, $"Resources/Data/{filePath}.json");
        // 지금은 알아보기 쉽기 위해 true지만 빌드때는 false로 해야 함
        string jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, jsonString);
    }

    // 해킹 방지로 하드 코딩과 암호화 중에 고민 중
    public Data.GameData CreateDefaultGameData()
    {
        return new Data.GameData
        {
            info = new Data.Info
            {
                coin = 10000,
                HP = 100.0f,
                lastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            stats = new List<Data.Stat>
            {
                new Data.Stat { statType = "MaxHP", statIcon = "HPIcon", statLevel = 1, statName = "최대 체력", 
                    statValue = 100.0f, statIncreaseValue = 10.0f, statPrice = 1, statIncreasePrice = 1 },
                new Data.Stat { statType = "Regeneration", statIcon = "RegenerationIcon", statLevel = 1, statName = "자연 회복", 
                    statValue = 5.0f, statIncreaseValue = 1.0f, statPrice = 10, statIncreasePrice = 10 },
                new Data.Stat { statType = "ATK", statIcon = "ATKIcon", statLevel = 1, statName = "공격력", 
                    statValue = 10.0f, statIncreaseValue = 0.5f, statPrice = 1, statIncreasePrice = 2 },
                new Data.Stat { statType = "DEF", statIcon = "DEFIcon", statLevel = 1, statName = "방어력", 
                    statValue = 1.0f, statIncreaseValue = 0.5f, statPrice = 10, statIncreasePrice = 10 },
                new Data.Stat { statType = "AttackSpeed", statIcon = "AttackSpeedIcon", statLevel = 1, statName = "공격 속도", 
                    statValue = 1.0f, statIncreaseValue = 0.01f, statPrice = 10, statIncreasePrice = 2 },
                new Data.Stat { statType = "Range", statIcon = "RangeIcon", statLevel = 1, statName = "공격 범위", 
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
