using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

//public interface ILoader<Key, Value>
//{
//    Dictionary<Key, Value> MakeDict();
//}

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

    private string _path;

    public bool GameDataReady { get; private set; } = false;

    //public void Init()
    //{
    //    _path = Application.persistentDataPath;
    //    MyPlayerStatDict = LoadJson<Data.StatData, string, Data.Stat>("MyPlayerStatData").MakeDict();
    //    EnemyDict = LoadJson<Data.EnemyData, string, Data.Enemy>("EnemyData").MakeDict();
    //}

    //// �� �κ� �� �𸣰���
    //Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    //{
    //    //string textAsset = File.ReadAllText(Path.Combine(_path, $"{path}.json"));
    //    TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<Loader>(textAsset.text);
    //}

    //// SaveJson<Loader, Key, Value>(Loader data, string path)
    //public void SaveJson<T>(T data, string path)
    //{
    //    // ��ü�� JSON ���ڿ��� ��ȯ
    //    string jsonString = JsonUtility.ToJson(data, true);

    //    // ������ ���� ��� ����
    //    //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
    //    string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

    //    // JSON ���Ϸ� ����
    //    File.WriteAllText(filePath, jsonString);

    //    //Debug.Log($"JSON ���� �Ϸ�: {filePath}");
    //}

    public async UniTask Init()
    {
        //_path = Path.Combine(Application.persistentDataPath, "GameData.json");
        _path = "GameData";

        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);
        Data.GameData gameData = await LoadGameData();
        MyPlayerInfo = gameData.info;
        MyPlayerStatDict = gameData.MakeDict(gameData.stats, stat => stat.statType);
        EnemyDict = gameData.MakeDict(gameData.enemys, enemy => enemy.enemyName);
        GameDataReady = true;
    }

    // ���� private�� ������ �̷��� ���� �� �ʿ䰡 �ֳ� ��� ��
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

        gameData = _localData.LoadLocalData<Data.GameData>(_path);
        if (gameData != null)
            return gameData;

        Debug.Log("����� �����Ͱ� �����ϴ�. �⺻ �����͸� �����մϴ�.");
        gameData = _localData.CreateDefaultGameData();
        if (gameData != null)
        {
            SaveGameData(gameData);
            return gameData;
        }

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
            _localData.SaveLocalData(data, "GameDataTest");  // ���� ����
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
        Init().Forget();
    }
}
