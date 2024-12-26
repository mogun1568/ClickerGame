using UnityEngine;

public class MyPlayerController : CreatureController
{
    private string _enemyTag = "Enemy";
    private GameObject _target;

    protected override void Init()
    {
        Managers.Game.MyPlayer = this;

        Stat.HP = 100;
        Stat.ATK = 10;
        Stat.DEF = 50;
        Stat.Range = 1.5f;
        Stat.AttackSpeed = 1;
        Stat.AttackCountDown = 1;

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float disToEnemy = enemy.transform.position.x - transform.position.x;
            //Debug.Log(disToEnemy); 

            if (disToEnemy <= Stat.Range)
            {
                nearestEnemy = enemy;
                break;
            }
        }

        if (nearestEnemy != null)
            _target = nearestEnemy;
        else
            _target = null;
    }

    private void Update()
    {
        if (_target == null)
        {
            State = Define.State.Run;
            return;
        }
          
        State = Define.State.Attack;

        if (Stat.AttackCountDown <= 0f)
        {
            Attack(_target, Stat.ATK);
            Stat.AttackCountDown = 1f / Stat.AttackSpeed;
        }

        Stat.AttackCountDown -= Time.deltaTime;
    }
}
