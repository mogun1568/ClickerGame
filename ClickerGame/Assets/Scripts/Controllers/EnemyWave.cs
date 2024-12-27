using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    //private int _enemyCount;
    private int _waveCount = 3;

    private void Update()
    {
        if (Managers.Game._enemyCount > 0)
            return;

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < _waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }   

    void SpawnEnemy()
    {
        Managers.Resource.Instantiate($"Enemy/HeavyBandit");
        Managers.Game._enemyCount++;
    }
}
