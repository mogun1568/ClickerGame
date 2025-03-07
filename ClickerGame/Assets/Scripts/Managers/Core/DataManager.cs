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
    private LocalDataManager _localData;
    private FirebaseDataManager _firebaseData;

    public LocalDataManager LocalData
    {
        get
        {
            if (_localData == null)  // 처음 접근할 때만 생성
                _localData = new LocalDataManager();
            return _localData;
        }
    }

    public FirebaseDataManager FirebaseData
    {
        get
        {
            if (_firebaseData == null)  // 처음 접근할 때만 생성
                _firebaseData = new FirebaseDataManager();
            return _firebaseData;
        }
    }
    #endregion

    public Dictionary<string, Data.Stat> MyPlayerStatDict { get; private set; } = new Dictionary<string, Data.Stat>();
    public Dictionary<string, Data.Enemy> EnemyDict { get; private set; } = new Dictionary<string, Data.Enemy>();

    private string _path;

    public bool IsLogIn { get; private set; } = false;

    public bool GameDataReady { get; private set; } = false;

    //public void Init()
    //{
    //    _path = Application.persistentDataPath;
    //    MyPlayerStatDict = LoadJson<Data.StatData, string, Data.Stat>("MyPlayerStatData").MakeDict();
    //    EnemyDict = LoadJson<Data.EnemyData, string, Data.Enemy>("EnemyData").MakeDict();
    //}

    //// 이 부분 잘 모르겠음
    //Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    //{
    //    //string textAsset = File.ReadAllText(Path.Combine(_path, $"{path}.json"));
    //    TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<Loader>(textAsset.text);
    //}

    //// SaveJson<Loader, Key, Value>(Loader data, string path)
    //public void SaveJson<T>(T data, string path)
    //{
    //    // 객체를 JSON 문자열로 변환
    //    string jsonString = JsonUtility.ToJson(data, true);

    //    // 저장할 파일 경로 설정
    //    //string filePath = Path.Combine(Application.persistentDataPath, $"{path}.json");
    //    string filePath = Path.Combine(Application.dataPath, $"Resources/Data/{path}.json");

    //    // JSON 파일로 저장
    //    File.WriteAllText(filePath, jsonString);

    //    //Debug.Log($"JSON 저장 완료: {filePath}");
    //}

    public async UniTask Init()
    {
        //_path = Path.Combine(Application.persistentDataPath, "GameData.json");
        _path = "GameData";

        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);
        Data.GameData gameData = await LoadGameData();
        MyPlayerStatDict = gameData.MakeDict(gameData.stats, stat => stat.statType);
        EnemyDict = gameData.MakeDict(gameData.enemys, enemy => enemy.enemyName);
        GameDataReady = true;
    }

    public async UniTask<Data.GameData> LoadGameData()
    {
        Data.GameData gameData = null;

        if (Managers.Firebase.IsLogIn)
        {
            gameData = await FirebaseData.LoadGameData();
            if (gameData != null)
                return gameData;
            else
                Debug.Log("Firebase 데이터가 없어서 기본 로컬 데이터 로드!");
        }

        gameData = LocalData.LoadLocalData<Data.GameData>(_path);
        if (gameData != null)
            return gameData;

        Debug.Log("저장된 데이터가 없습니다. 기본 데이터를 생성합니다.");
        gameData = LocalData.CreateDefaultGameData();
        if (gameData != null)
        {
            SaveGameData(gameData);
            return gameData;
        }

        Debug.LogWarning("기본 데이터 생성을 실패했습니다.");
        return null;
    }


    public void SaveGameData(Data.GameData data)
    {
        if (Managers.Firebase.IsLogIn)
        {
            FirebaseData.SaveGameData(data).Forget();  // UniTask 변환
        }
        else
        {
            LocalData.SaveLocalData(data, "GameDataTest");  // 로컬 저장
        }
    }

    private void UpdateStat(string statType)
    {
        Dictionary<string, object> StatValues = new Dictionary<string, object>
        {
            { "statValue", MyPlayerStatDict[statType].statValue },
            { "statLevel", MyPlayerStatDict[statType].statLevel },
            { "statPrice", MyPlayerStatDict[statType].statPrice }
        };
        FirebaseData.UpdateStat("Speed", StatValues).Forget();
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
        {
            SaveGameData(ReturnGameData());
        }
    }

    public Data.GameData ReturnGameData()
    {
        return new Data.GameData
        {
            stats = UpdateStats(Managers.Game.MyPlayer),
            enemys = UpdateEnemys()
        };
    }

    private List<Data.Stat> UpdateStats(MyPlayerController _myPlayer)
    {
        MyPlayerStatDict["Coin"].statValue = _myPlayer.StatInfo.Coin;
        MyPlayerStatDict["MaxHP"].statValue = _myPlayer.MaxHP;
        MyPlayerStatDict["HP"].statValue = _myPlayer.HP;
        MyPlayerStatDict["Regeneration"].statValue = _myPlayer.Regeneration;
        MyPlayerStatDict["ATK"].statValue = _myPlayer.StatInfo.ATK;
        MyPlayerStatDict["DEF"].statValue = _myPlayer.StatInfo.DEF;
        MyPlayerStatDict["AttackSpeed"].statValue = _myPlayer.AttackSpeed;
        MyPlayerStatDict["Range"].statValue = _myPlayer.StatInfo.Range;

        return new List<Data.Stat>(Managers.Data.MyPlayerStatDict.Values);
    }

    private List<Data.Enemy> UpdateEnemys()
    {
        // 몬스터 체력이나 공격력을 플레이어의 스탯에 따라 변화하게 할 지
        // 아니면 시간? 라운드? 등에 따라 다르게 할 지 고민 중

        return new List<Data.Enemy>(Managers.Data.EnemyDict.Values);
    }

    public void Clear()
    {
        GameDataReady = false;
        Init().Forget();
    }
}
