using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataManager
{
    #region Datas
    private LocalDataManager _localData = new LocalDataManager();
    private FirebaseDataManager _firebaseData = new FirebaseDataManager();

    //public LocalDataManager LocalData
    //{
    //    get
    //    {
    //        if (_localData == null)  // ó�� ������ ���� ����
    //            _localData = new LocalDataManager();
    //        return _localData;
    //    }
    //}

    //public FirebaseDataManager FirebaseData
    //{
    //    get
    //    {
    //        if (_firebaseData == null)  // ó�� ������ ���� ����
    //            _firebaseData = new FirebaseDataManager();
    //        return _firebaseData;
    //    }
    //}
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
        MyPlayerStatDict = gameData.MakeDict(gameData.stats, stat => stat.statType);
        EnemyDict = gameData.MakeDict(gameData.enemys, enemy => enemy.enemyName);
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
                Debug.Log("Firebase �����Ͱ� ��� �⺻ ���� ������ �ε�!");
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


    public void SaveGameData(Data.GameData data)
    {
        if (Managers.Firebase.IsLogIn)
        {
            _firebaseData.SaveGameData(data).Forget();  // UniTask ��ȯ
        }
        else
        {
            _localData.SaveLocalData(data);  // ���� ����
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
            SaveGameData(ReturnGameData());
    }

    private void UpdateStat(string statType)
    {
        Dictionary<string, object> StatValues = new Dictionary<string, object>
        {
            { "statValue", MyPlayerStatDict[statType].statValue },
            { "statLevel", MyPlayerStatDict[statType].statLevel },
            { "statPrice", MyPlayerStatDict[statType].statPrice }
        };
        _firebaseData.UpdateStat("Speed", StatValues).Forget();
    }

    public void UpdateDict(string statType = "")
    {
        if (Managers.Firebase.IsLogIn)
        {
            if (statType == "")
                SaveGameData(ReturnGameData());
            else
                UpdateStat(statType);
        }
        else
            SaveGameData(ReturnGameData());
    }

    private Data.GameData ReturnGameData()
    {
        return new Data.GameData
        {
            info = SaveInfo(),
            stats = SaveStats(),
            enemys = SaveEnemys()
        };
    }

    private Data.Info SaveInfo()
    {
        return new Data.Info
        {
            coin = MyPlayerInfo.coin,
            HP = MyPlayerInfo.HP,
            lastTime = UpdateLastTime()
        };
    }

    private List<Data.Stat> SaveStats()
    {
        return new List<Data.Stat>(MyPlayerStatDict.Values);
    }

    private List<Data.Enemy> SaveEnemys()
    {
        // ���� ü���̳� ���ݷ��� �÷��̾��� ���ȿ� ���� ��ȭ�ϰ� �� ��
        // �ƴϸ� �ð�? ����? � ���� �ٸ��� �� �� ��� ��

        return new List<Data.Enemy>(EnemyDict.Values);
    }

    private long UpdateLastTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // �����Ϳ����� test json ���Ͽ� ����Ǿ ���� �Ŀ� �׽�Ʈ �ʿ�
    public void OfflineReward()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int diffTime = (int)(currentTime - MyPlayerInfo.lastTime);

        // 1��(60��) �̻� ���̰� ���� �������� ���� UI ����
        if (diffTime < 60)
            return;

        int coin = diffTime / 60 * 1;
        Managers.UI.ShowPopupUI<UI_Offline>("Popup_Offline").StatInit(coin);
    }

    public void Clear()
    {
        GameDataReady = false;
        InitAsync().Forget();
    }
}
