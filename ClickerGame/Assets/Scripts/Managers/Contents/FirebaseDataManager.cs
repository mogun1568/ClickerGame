using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

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
                Data.GameData gameData = JsonUtility.FromJson<Data.GameData>(json);
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

    // 전체 데이터 저장 (덮어쓰기)
    public async UniTask SaveGameData(Data.GameData data)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;
        string jsonData = JsonUtility.ToJson(data);

        try
        {
            await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).AsUniTask();
            Debug.Log("Game data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    // 특정 스탯 업데이트 (일부 필드만 수정)
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

            await dbReference.Child("users").Child(userId).Child("stats").UpdateChildrenAsync(updates).AsUniTask();
            Debug.Log($"Stat '{statType}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update stat '{statType}': {e.Message}");
        }
    }

    // 특정 적 업데이트 (일부 필드만 수정)
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

    // 적 추가 (Push 키 활용)
    public void AddNewEnemy(Data.Enemy newEnemy)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        AddNewEnemyAsync(userId, newEnemy).Forget(); // async 사용 안 하고 호출 가능
    }

    private async UniTask AddNewEnemyAsync(string userId, Data.Enemy newEnemy)
    {
        try
        {
            DatabaseReference enemiesRef = dbReference.Child("users").Child(userId).Child("enemys");
            string enemyKey = enemiesRef.Push().Key; // 고유 키 생성
            await enemiesRef.Child(enemyKey).SetValueAsync(newEnemy).AsUniTask(); // 비동기 저장

            Debug.Log($"Enemy '{newEnemy.enemyName}' added successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to add enemy: {e.Message}");
        }
    }
}
