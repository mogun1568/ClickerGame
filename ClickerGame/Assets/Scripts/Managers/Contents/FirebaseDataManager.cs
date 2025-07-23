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
                Logging.Log("Game data loaded successfully.");
                return gameData;
            }
            else
            {
                Logging.LogWarning("No game data found.");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to load game data: {e.Message}");
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
            Logging.Log("Game data saved successfully.");
            Managers.Data.CheckSaveDataDone = true;
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to save game data: {e.Message}");
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

            Logging.Log($"Info field '{fieldName}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to update info field '{fieldName}': {e.Message}");
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

            Logging.Log($"Stat '{statType}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to update stat '{statType}': {e.Message}");
        }
    }

    public async UniTask UpdateSkill(string skillType, Dictionary<string, object> skillValues)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;

        try
        {
            Dictionary<string, object> updates = new Dictionary<string, object>();
            foreach (var kvp in skillValues)
            {
                updates[$"{skillType}/{kvp.Key}"] = kvp.Value;
            }

            await dbReference.Child("users").Child(userId).Child("skills").Child(skillType)
                .UpdateChildrenAsync(skillValues).AsUniTask();

            Logging.Log($"Skill '{skillType}' updated successfully.");
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to update skill '{skillType}': {e.Message}");
        }
    }

    public async UniTask AddSkin(string classType, string skinName)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;
        var classRef = dbReference.Child("users").Child(userId).Child("skins").Child(classType);

        try
        {
            // 기존 리스트 불러오기
            DataSnapshot snapshot = await classRef.GetValueAsync().AsUniTask();
            List<string> skinList = new List<string>();

            if (snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    if (child.Value != null)
                        skinList.Add(child.Value.ToString());
                }
            }

            // 이미 있으면 추가 안 함
            if (skinList.Contains(skinName))
            {
                Logging.Log($"스킨 '{skinName}'은(는) 이미 보유 중입니다.");
                return;
            }

            skinList.Add(skinName);
            await classRef.SetValueAsync(skinList).AsUniTask();

            Logging.Log($"스킨 '{skinName}' 추가 완료");
        }
        catch (System.Exception e)
        {
            Logging.LogError($"스킨 추가 실패: {e.Message}");
        }
    }

    public async UniTask<List<Data.RankingData>> FetchAllUsersRankingData()
    {
        List<Data.RankingData> rankingList = new List<Data.RankingData>();

        try
        {
            DataSnapshot snapshot = await dbReference.Child("users").GetValueAsync().AsUniTask();

            if (!snapshot.Exists || snapshot.ChildrenCount == 0)
                return rankingList;

            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                string userId = userSnapshot.Key;

                var infoSnapshot = userSnapshot.Child("info");
                if (!infoSnapshot.Exists)
                {
                    Logging.LogWarning($"User '{userId}'에 'info' 노드가 없습니다.");
                    continue;
                }

                if (!(infoSnapshot.HasChild("Nickname") && infoSnapshot.HasChild("Reincarnation")
                    && infoSnapshot.HasChild("Round")))
                {
                    Logging.LogWarning($"User '{userId}'에 'info' 노드에 키가 없습니다.");
                    continue;
                }

                string nickname = infoSnapshot.Child("Nickname").Value?.ToString() ?? "";
                int reincarnation = int.TryParse(infoSnapshot.Child("Reincarnation").Value?.ToString(), out var r1) ? r1 : 0;
                int round = int.TryParse(infoSnapshot.Child("Round").Value?.ToString(), out var r2) ? r2 : 0;

                rankingList.Add(new Data.RankingData
                {
                    userId = userId,
                    nickname = nickname,
                    reincarnation = reincarnation,
                    round = round
                });

                Logging.Log("Successfully fetched ranking data.");
            }
        }
        catch (System.Exception e)
        {
            Logging.LogError($"Failed to fetch ranking data: {e.Message}");
        }

        return rankingList;
    }
}
