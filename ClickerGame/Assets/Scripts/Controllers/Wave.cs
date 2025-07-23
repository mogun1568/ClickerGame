using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private Data.Info _myPlayerInfo = null;

    public int _enemyCount;
    private int _enemyWaveCount;
    private int _bossWaveCount;

    void Start()
    {
        InitAsync().Forget();
    }

    private async UniTask InitAsync()
    {
        Managers.Game.Wave = this;
        _enemyCount = 0;
        _enemyWaveCount = 3;
        _bossWaveCount = 1;

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
        _myPlayerInfo = Managers.Data.MyPlayerInfo;

        Managers.Game.CurMap = Managers.Resource.Instantiate($"Map/{_myPlayerInfo.Map}");
        if (_myPlayerInfo.Map == "Plain")
            Managers.Sound.Play("A Great Journey - Overworld", Define.Sound.BGM);
        else
            Managers.Sound.Play("Magic Elderwood Forest - Overworld", Define.Sound.BGM);

        float spawnPosY = Managers.Resource.SkinItemDict[_myPlayerInfo.Skin].spawnPosY;
        Managers.Resource.Instantiate($"Player/{_myPlayerInfo.Class}/{_myPlayerInfo.Skin}", new Vector3(-2, spawnPosY, -1));
        
    }

    private void Update()
    {
        if (!Managers.Data.GameDataReady)
            return;

        if (_enemyCount > 0)
            return;

        if (_myPlayerInfo == null)
            return;

        _myPlayerInfo.Round++;

        MapChange();

        if (_myPlayerInfo.Round % 10 == 0)
            StartCoroutine(SpawnBossWave());
        else
            StartCoroutine(SpawnEnemyWave());
    }

    private void MapChange()
    {
        if (_myPlayerInfo.Round % 100 > 0 && _myPlayerInfo.Round % 100 <= 50)
        {
            if (_myPlayerInfo.Map == "Plain")
                return;

            _myPlayerInfo.Map = "Plain";
            Managers.Sound.Play("A Great Journey - Overworld", Define.Sound.BGM);
        }
        else
        {
            if (_myPlayerInfo.Map == "Forest")
                return;

            _myPlayerInfo.Map = "Forest";
            Managers.Sound.Play("Magic Elderwood Forest - Overworld", Define.Sound.BGM);
        }

        GameObject newMap = Managers.Resource.Instantiate($"Map/{_myPlayerInfo.Map}");

        // 이전 맵을 언제 제거할 지 정해야 함 (100라운드에 제거?)
        if (Managers.Game.PreMap != null)
            Managers.Resource.Destroy(Managers.Game.PreMap);
        Managers.Game.PreMap = Managers.Game.CurMap;
        Managers.Game.CurMap = newMap;

        // 위치 변경은 스크립트 따로 만들어서 Bind 이용해서 할까 고민
        Vector3 localPos1 = newMap.transform.GetChild(0).localPosition;
        localPos1.x = 15;
        newMap.transform.GetChild(0).localPosition = localPos1;

        Vector3 localPos2 = newMap.transform.GetChild(1).localPosition;
        localPos2.x = 30;
        newMap.transform.GetChild(1).localPosition = localPos2;

        Vector3 localPos3 = newMap.transform.GetChild(2).GetChild(0).localPosition;
        localPos3.x = 15;
        newMap.transform.GetChild(2).GetChild(0).localPosition = localPos3;

        Vector3 localPos4 = newMap.transform.GetChild(2).GetChild(1).localPosition;
        localPos4.x = 30;
        newMap.transform.GetChild(2).GetChild(1).localPosition = localPos4;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn(0f));
    }

    protected IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        float spawnPosY = Managers.Resource.SkinItemDict[_myPlayerInfo.Skin].spawnPosY;
        Managers.Resource.Instantiate($"Player/{_myPlayerInfo.Class}/{_myPlayerInfo.Skin}", new Vector3(-7, spawnPosY, -1));
        
    }

    // 적 종류 추가 방식은 플레이어의 Reincarnation과 Round를 이용해 할 예정
    IEnumerator SpawnEnemyWave()
    {
        _enemyCount += _enemyWaveCount;
        //Logging.Log(_enemyCount);

        yield return new WaitForSeconds(1f);

        Managers.Game.MyPlayer.StatInfo.AttackCountdown = 0;

        for (int i = 0; i < _enemyWaveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }   

    void SpawnEnemy()
    {
        if (_myPlayerInfo.Round % 100 > 0 && _myPlayerInfo.Round % 100 <= 50)
            Managers.Resource.Instantiate($"Enemy/LightBandit", new Vector3(7, 1.9f, -1));
        else
            Managers.Resource.Instantiate($"Enemy/HeavyBandit", new Vector3(7, 1.9f, -1));
    }

    IEnumerator SpawnBossWave()
    {
        _enemyCount += _bossWaveCount;

        yield return new WaitForSeconds(1f);

        Managers.Game.MyPlayer.StatInfo.AttackCountdown = 0;

        for (int i = 0; i < _bossWaveCount; i++)
        {
            SpawnBoss();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnBoss()
    {
        Managers.Resource.Instantiate($"Enemy/MedievalKing", new Vector3(7, 3.45f, -1));
    }
}
