using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Newtonsoft.Json;

public class FirebaseDataManager
{
    private FirebaseAuth auth;
    private DatabaseReference dbReference;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // 전체 데이터 불러오기
    public async UniTask<Data.GameData> LoadGameData()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return null;

        string userId = user.UserId;

        try
        {
            DataSnapshot snapshot = await dbReference.Child("users").Child(userId).GetValueAsync().AsUniTask();
            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                Data.GameData gameData = JsonConvert.DeserializeObject<Data.GameData>(json);
                Debug.Log("Game data loaded successfully.");
                return gameData;
            }
            else
            {
                Debug.LogWarning("No game data found.");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
            return null;
        }
    }

    // 전체 데이터 저장
    public async UniTask SaveGameData(Data.GameData data)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;
        string jsonData = JsonConvert.SerializeObject(data);

        try
        {
            await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).AsUniTask();
            Debug.Log("Game data saved successfully.");
            Managers.Data.CheckSaveDataDone = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    // 특정 인포 업데이트
    public async UniTask UpdateInfo(string fieldName, object fieldValue)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        try
        {
            await dbReference.Child("users").Child(userId).Child("info").Child(fieldName)
                .SetValueAsync(fieldValue).AsUniTask();

            Debug.Log($"Info field '{fieldName}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update info field '{fieldName}': {e.Message}");
        }
    }

    // 특정 스탯 업데이트
    public async UniTask UpdateStat(string statType, Dictionary<string, object> statValues)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        try
        {
            Dictionary<string, object> updates = new Dictionary<string, object>();
            foreach (var kvp in statValues)
            {
                updates[$"{statType}/{kvp.Key}"] = kvp.Value;
            }

            await dbReference.Child("users").Child(userId).Child("stats").Child(statType)
                .UpdateChildrenAsync(statValues).AsUniTask();

            Debug.Log($"Stat '{statType}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update stat '{statType}': {e.Message}");
        }
    }

    // 모든 적의 특정 스탯 업데이트
    public async UniTask UpdateAllEnemiesStat(string statName, object newValue)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        try
        {
            DatabaseReference enemiesRef = dbReference.Child("users").Child(userId).Child("enemys");
            DataSnapshot snapshot = await enemiesRef.GetValueAsync().AsUniTask();

            if (!snapshot.Exists)
            {
                Debug.LogWarning("No enemies found to update.");
                return;
            }

            Dictionary<string, object> enemyUpdates = new Dictionary<string, object>();
            foreach (DataSnapshot enemy in snapshot.Children)
            {
                enemyUpdates[$"{enemy.Key}/{statName}"] = newValue;
            }

            await enemiesRef.UpdateChildrenAsync(enemyUpdates).AsUniTask();
            Debug.Log($"Updated '{statName}' for all enemies successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update '{statName}' for all enemies: {e.Message}");
        }
    }

    // 적 추가 (enemyName을 Key로 사용)
    public void AddNewEnemy(Data.Enemy newEnemy)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        AddNewEnemyAsync(userId, newEnemy).Forget();
    }

    private async UniTask AddNewEnemyAsync(string userId, Data.Enemy newEnemy)
    {
        try
        {
            DatabaseReference enemiesRef = dbReference.Child("users").Child(userId).Child("enemys");

            await enemiesRef.Child(newEnemy.enemyName).SetValueAsync(newEnemy).AsUniTask();

            Debug.Log($"Enemy '{newEnemy.enemyName}' added successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to add enemy: {e.Message}");
        }
    }
}
