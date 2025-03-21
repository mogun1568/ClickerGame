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
    public Dictionary<string, Data.Stat> MyPlayerStatDict { get; private set; } = new Dictionary<string, Data.Stat>();
    public Dictionary<string, Data.Enemy> EnemyDict { get; private set; } = new Dictionary<string, Data.Enemy>();

    public bool GameDataReady { get; private set; } = false;

    public async UniTask InitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);
        Data.GameData gameData = await LoadGameData();
        MyPlayerInfo = gameData.info;
        MyPlayerStatDict = gameData.stats;
        EnemyDict = gameData.enemys;
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
            if (gameData != null)
                return gameData;
            else
            {
                Debug.LogError("Firebase �����Ͱ� �����ϴ�.");
                return null;
            }
        }

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
            if (infoType == "LastTime")
                _firebaseData.UpdateInfo(infoType, UpdateLastTime()).Forget();
            else
                _firebaseData.UpdateInfo(infoType, infoValue).Forget();
        }
        else
            SaveGameData();
    }

    private void UpdateStat(string statType)
    {
        Dictionary<string, object> StatValues = new Dictionary<string, object>
        {
            { "statValue", MyPlayerStatDict[statType].statValue },
            { "statLevel", MyPlayerStatDict[statType].statLevel },
            { "statPrice", MyPlayerStatDict[statType].statPrice }
        };
        _firebaseData.UpdateStat(statType, StatValues).Forget();
    }

    public void UpdateDict(string statType = "")
    {
        if (Managers.Firebase.IsLogIn)
        {
            if (statType == "")
                SaveGameData();
            else
                UpdateStat(statType);
        }
        else
            SaveGameData();
    }

    private Data.GameData ReturnGameData()
    {
        return new Data.GameData
        {
            info = MyPlayerInfo,
            stats = MyPlayerStatDict,
            enemys = EnemyDict
        };
    }

    private long UpdateLastTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // �����Ϳ����� test json ���Ͽ� ����Ǿ ���� �Ŀ� �׽�Ʈ �ʿ�
    public void OfflineReward()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int diffTime = (int)(currentTime - MyPlayerInfo.LastTime);

        // 1��(60��) �̻� ���̰� ���� �������� ���� UI ����
        if (diffTime < 60)
            return;

        int coin = diffTime / 60 * 1;
        Managers.UI.ShowPopupUI<UI_Offline>("Popup_Offline").StatInit(coin);
    }

    public void DeleteLocalData()
    {
        _localData.DeleteData();
    }

    public void Clear()
    {
        GameDataReady = false;
        InitAsync().Forget();
    }
}
