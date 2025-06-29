using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataManager
{
    #region Datas
    private LocalDataManager _localData = new LocalDataManager();
    private FirebaseDataManager _firebaseData = new FirebaseDataManager();
    #endregion

    public Data.Info MyPlayerInfo { get; private set; } = new Data.Info();
    public Dictionary<string, Data.StatInfo> MyPlayerStatDict { get; private set; } = new Dictionary<string, Data.StatInfo>();
    public Dictionary<string, Data.SkillInfo> MyPlayerSkillDict { get; private set; } = new Dictionary<string, Data.SkillInfo>();
    public Dictionary<Define.ClassType, List<string>> MyPlayerSkinDict { get; private set; } = new Dictionary<Define.ClassType, List<string>>();
    
    public bool GameDataReady { get; private set; } = false;
    public bool CheckSaveDataDone { get; set; } = false;

    public async UniTask InitAsync()
    {
        CheckSaveDataDone = false;
        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);
        Data.GameData gameData = await LoadGameData();
        MyPlayerInfo = gameData.info;
        MyPlayerStatDict = gameData.stats;
        MyPlayerSkillDict = gameData.skills;
        MyPlayerSkinDict = gameData.skins;
        Managers.Skill.Init();
        //await Managers.Ranking.InitAsync();
        MyPlayerInfo.Round--;
        GameDataReady = true;
    }

    public void FirebaseDataInit()
    {
        _firebaseData.Init();
    }

    public async UniTask<Data.GameData> LoadGameData()
    {
        Data.GameData gameData = null;

        if (Managers.Firebase.IsLogIn)
        {
            gameData = await _firebaseData.LoadGameData();
            if (gameData == null)
            {
                Debug.LogError("Firebase �����Ͱ� �����ϴ�.");

                gameData = LoadLocalData();
                if (gameData != null)
                {
                    CheckSaveDataDone = false;
                    SaveGameData();
                    await UniTask.WaitUntil(() => CheckSaveDataDone);
                }
                else
                {
                    Debug.LogError("���� �����͸� �������� ���߽��ϴ�.");
                }
            }

            _localData.DeleteData();

            return gameData;
        }

        gameData = LoadLocalData();
        if (gameData != null)
            return gameData;

        return null;
    }

    private Data.GameData LoadLocalData()
    {
        Data.GameData gameData = null;

        gameData = _localData.LoadLocalData<Data.GameData>();
        if (gameData != null)
            return gameData;

        Debug.Log("����� �����Ͱ� �����ϴ�. �⺻ �����͸� �����մϴ�.");
        gameData = _localData.CreateDefaultGameData();
        if (gameData != null)
            return gameData;

        Debug.LogWarning("�⺻ ������ ������ �����߽��ϴ�.");
        return null;
    }

    public void SaveGameData()
    {
        UpdateLastTime();

        if (Managers.Firebase.IsLogIn)
        {
            _firebaseData.SaveGameData(ReturnGameData()).Forget();  // UniTask ��ȯ
        }
        else
        {
            _localData.SaveLocalData(ReturnGameData());  // ���� ����
        }
    }

    public void UpdateInfo(string infoType, object infoValue = null)
    {
        if (Managers.Firebase.IsLogIn)
        {
            _firebaseData.UpdateInfo(infoType, infoValue).Forget();
        }
        else
            SaveGameData();
    }

    private void UpdateFirebaseStat(string statType)
    {
        Dictionary<string, object> StatValues = new Dictionary<string, object>
        {
            { "statLevel", MyPlayerStatDict[statType].statLevel },
            { "statValue", MyPlayerStatDict[statType].statValue },
            { "statPrice", MyPlayerStatDict[statType].statPrice }
        };
        _firebaseData.UpdateStat(statType, StatValues).Forget();
    }

    public void UpdateStat(string statType = "")
    {
        if (Managers.Firebase.IsLogIn)
        {
            if (statType == "")
                SaveGameData();
            else
                UpdateFirebaseStat(statType);
        }
        else
            SaveGameData();
    }

    private void UpdateFirebaseSkill(string skillType)
    {
        Dictionary<string, object> SKillValues = new Dictionary<string, object>
        {
            { "skillLevel", MyPlayerSkillDict[skillType].skillLevel },
            { "skillValue", MyPlayerSkillDict[skillType].skillValue }
        };
        _firebaseData.UpdateSkill(skillType, SKillValues).Forget();
    }

    public void UpdateSKill(string skillType = "")
    {
        if (Managers.Firebase.IsLogIn)
        {
            if (skillType == "")
                SaveGameData();
            else
                UpdateFirebaseSkill(skillType);
        }
        else
            SaveGameData();
    }

    public bool HasSkin(Define.ClassType classType, string skinName)
    {
        return MyPlayerSkinDict.TryGetValue(classType, out var skinList) && skinList.Contains(skinName);
    }

    public void AddSkin(Define.ClassType classType, string skinName)
    {
        if (HasSkin(classType, skinName))
            return;

        if (Managers.Firebase.IsLogIn)
            _firebaseData.AddSkin(classType.ToString(), skinName).Forget();
        else
        {
            List<string> skinList = MyPlayerSkinDict[classType];
            skinList.Add(skinName);
            SaveGameData();
        }
            
    }

    private Data.GameData ReturnGameData()
    {
        return new Data.GameData
        {
            info = MyPlayerInfo,
            stats = MyPlayerStatDict,
            skills = MyPlayerSkillDict,
            skins = MyPlayerSkinDict
        };
    }

    public void UpdateLastTime()
    {
        MyPlayerInfo.LastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // �����Ϳ����� test json ���Ͽ� ����Ǿ ���� �Ŀ� �׽�Ʈ �ʿ�
    public void OfflineReward()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int diffTime = (int)(currentTime - MyPlayerInfo.LastTime);

        // 1��(60��) �̻� ���̰� ���� �������� ���� UI ����
        if (diffTime < 60 && MyPlayerInfo.OfflineReward <= 0)
            return;

        MyPlayerInfo.OfflineReward += (diffTime / 60 * 1);
        Managers.UI.ShowPopupUI<UI_Offline>("Popup_Offline").StatInit();
    }

    public bool HasLocalData()
    {
        if (_localData.HasData()) return true;
        return false;
    }

    public void DoReincarnation(bool isReincarnation)
    {
        MyPlayerInfo = new Data.Info
        {
            Nickname = MyPlayerInfo.Nickname,
            Reincarnation = MyPlayerInfo.Reincarnation,
            Coin = 10000,
            Round = 1,
            Map = "Plain",
            Class = MyPlayerInfo.Class,
            Skin = MyPlayerInfo.Skin,
            HP = 100.0f,
            LastTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            OfflineReward = 0
        };

        if (isReincarnation)
            MyPlayerInfo.Reincarnation++;

        RestoreStat("MaxHP");
        RestoreStat("Regeneration");
        RestoreStat("ATK");
        RestoreStat("DEF");
        RestoreStat("AttackSpeed");
        RestoreStat("Range");

        // ���� �ʱ�ȭ

        SaveGameData();

        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }

    private void RestoreStat(string statKind)
    {
        MyPlayerStatDict[statKind].statLevel = Managers.Resource.StatDict[statKind].abilityLevel;
        MyPlayerStatDict[statKind].statValue = ReincarnationStatValue(statKind);
        MyPlayerStatDict[statKind].statPrice = Managers.Resource.StatDict[statKind].statPrice;
    }

    private float ReincarnationStatValue(string statKind)
    {
        return Managers.Resource.StatDict[statKind].abilityValue + 
            5 * MyPlayerInfo.Reincarnation * Managers.Resource.StatDict[statKind].abilityIncreaseValue;
    }

    public async UniTask<List<Data.RankingData>> LoadRanking()
    {
        return await _firebaseData.FetchAllUsersRankingData();
    }

    public void Clear()
    {
        GameDataReady = false;
        CheckSaveDataDone = false;
    }
}
