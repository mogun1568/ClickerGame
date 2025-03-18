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
    //        if (_localData == null)  // 처음 접근할 때만 생성
    //            _localData = new LocalDataManager();
    //        return _localData;
    //    }
    //}

    //public FirebaseDataManager FirebaseData
    //{
    //    get
    //    {
    //        if (_firebaseData == null)  // 처음 접근할 때만 생성
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
                Debug.Log("Firebase 데이터가 없어서 기본 로컬 데이터 로드!");
        }

        gameData = _localData.LoadLocalData<Data.GameData>();
        if (gameData != null)
            return gameData;

        Debug.Log("저장된 데이터가 없습니다. 기본 데이터를 생성합니다.");
        gameData = _localData.CreateDefaultGameData();
        if (gameData != null)
            return gameData;

        Debug.LogWarning("기본 데이터 생성을 실패했습니다.");
        return null;
    }


    public void SaveGameData(Data.GameData data)
    {
        if (Managers.Firebase.IsLogIn)
        {
            _firebaseData.SaveGameData(data).Forget();  // UniTask 변환
        }
        else
        {
            _localData.SaveLocalData(data);  // 로컬 저장
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
        // 몬스터 체력이나 공격력을 플레이어의 스탯에 따라 변화하게 할 지
        // 아니면 시간? 라운드? 등에 따라 다르게 할 지 고민 중

        return new List<Data.Enemy>(EnemyDict.Values);
    }

    private long UpdateLastTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // 에디터에서는 test json 파일에 저장되어서 빌드 후에 테스트 필요
    public void OfflineReward()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int diffTime = (int)(currentTime - MyPlayerInfo.lastTime);

        // 1분(60초) 이상 차이가 나면 오프라인 보상 UI 띄우기
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
