using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int _enemyCount;
    private int _waveCount;

    private void Start()
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
        StartCoroutine(Respawn(1f));
    }

    protected IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        Managers.Resource.Instantiate($"Player/HeroKnight", new Vector3(-7, 1.9f, -1));
        
    }

    IEnumerator SpawnEnemyWave()
    {
        for (int i = 0; i < _waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }   

    void SpawnEnemy()
    {
        Managers.Resource.Instantiate($"Enemy/HeavyBandit", new Vector3(7, 1.9f, -1));
        _enemyCount++;
    }
}
