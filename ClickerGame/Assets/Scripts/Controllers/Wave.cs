using System.Collections;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int _enemyCount;
    private int _waveCount;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.Game.Wave = this;
        _enemyCount = 0;
        _waveCount = 3;
        Managers.Resource.Instantiate($"Player/HeroKnight", new Vector3(-2, 1.9f, -1));
    }

    private void Update()
    {
        if (!Managers.Data.GameDataReady)
            return;

        if (_enemyCount > 0)
            return;

        StartCoroutine(SpawnEnemyWave());
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn(0f));
    }

    protected IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Managers.Resource.Instantiate($"Player/HeroKnight", new Vector3(-7, 1.9f, -1));
        
    }

    // 적 종류 추가 방식은 플레이어의 Reincarnation과 Round를 이용해 할 예정
    IEnumerator SpawnEnemyWave()
    {
        Managers.Data.MyPlayerInfo.Round++;

        _enemyCount += _waveCount;
        //Debug.Log(_enemyCount);

        yield return new WaitForSeconds(1f);

        Managers.Game.MyPlayer.StatInfo.AttackCountdown = 0;

        for (int i = 0; i < _waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }   

    void SpawnEnemy()
    {
        if (Managers.Data.MyPlayerInfo.Round < 50)
            Managers.Resource.Instantiate($"Enemy/LightBandit", new Vector3(7, 1.9f, -1));
        else
            Managers.Resource.Instantiate($"Enemy/HeavyBandit", new Vector3(7, 1.9f, -1));
    }
}
