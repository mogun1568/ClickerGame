using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// 게임 데이터 모델
[Serializable]
public class GameData
{
    public Dictionary<string, List<Data.Stat>> stats = new Dictionary<string, List<Data.Stat>>();
    public Dictionary<string, List<Data.Enemy>> enemys = new Dictionary<string, List<Data.Enemy>>();
}

public class FirebaseDataManager
{
    private FirebaseAuth auth;
    private DatabaseReference dbReference;

    public void Init()
    {
        auth = Managers.Firebase.auth;
        dbReference = Managers.Firebase.dbReference;
    }

    private bool CheckFirebase()
    {
        if (auth == null || dbReference == null)
        {
            //AddToInformation("Firebase is not initialized properly.");
            return false;
        }
        return true;
    }

    // 전체 데이터 저장 (덮어쓰기)
    public void SaveGameData(GameData data)
    {
        if (!CheckFirebase()) return;
        FirebaseUser user = auth.CurrentUser;
        if (user == null)
        {
            //AddToInformation("Login required.");
            return;
        }

        string userId = user.UserId;
        string jsonData = JsonUtility.ToJson(data);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            //if (task.IsCompletedSuccessfully)
            //    AddToInformation("Game data saved successfully.");
            //else
            //    AddToInformation("Failed to save game data: " + task.Exception);
        });
    }

    // 특정 스탯 저장
    public void SaveStat(string playerId, Data.Stat stat)
    {
        if (!CheckFirebase()) return;
        FirebaseUser user = auth.CurrentUser;
        if (user == null)
        {
            //AddToInformation("Login required.");
            return;
        }

        string userId = user.UserId;
        string jsonStat = JsonUtility.ToJson(stat);

        dbReference.Child("users").Child(userId).Child("stats").Child(playerId).Push().SetRawJsonValueAsync(jsonStat)
            .ContinueWith(task =>
            {
                //if (task.IsCompletedSuccessfully)
                //    AddToInformation("Stat saved successfully.");
                //else
                //    AddToInformation("Failed to save stat: " + task.Exception);
            });
    }

    // 특정 적 데이터 저장
    public void SaveEnemy(string playerId, Data.Enemy enemy)
    {
        if (!CheckFirebase()) return;
        FirebaseUser user = auth.CurrentUser;
        if (user == null)
        {
            //AddToInformation("Login required.");
            return;
        }

        string userId = user.UserId;
        string jsonEnemy = JsonUtility.ToJson(enemy);

        dbReference.Child("users").Child(userId).Child("enemys").Child(playerId).Push().SetRawJsonValueAsync(jsonEnemy)
            .ContinueWith(task =>
            {
                //if (task.IsCompletedSuccessfully)
                //    AddToInformation("Enemy saved successfully.");
                //else
                //    AddToInformation("Failed to save enemy: " + task.Exception);
            });
    }

    // 전체 데이터 불러오기
    public void LoadGameData()
    {
        if (!CheckFirebase()) return;
        FirebaseUser user = auth.CurrentUser;
        if (user == null)
        {
            //AddToInformation("Login required.");
            return;
        }

        string userId = user.UserId;
        dbReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully && task.Result.Exists)
            {
                string json = task.Result.GetRawJsonValue();
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                //AddToInformation("Game data loaded successfully.");
            }
            else
            {
                //AddToInformation("No game data found.");
            }
        });
    }
}

