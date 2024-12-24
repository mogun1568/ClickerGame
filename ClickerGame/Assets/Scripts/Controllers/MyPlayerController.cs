using UnityEngine;

public class MyPlayerController : CreatureController
{
    private string enemyTag = "Enemy";
    //[SerializeField]
    private float attackDis = 1.5f;
    private bool flag = false;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        flag = false;

        foreach (GameObject enemy in enemies)
        {
            float disToEnemy = enemy.transform.position.x - transform.position.x;
            //Debug.Log(disToEnemy); 

            if (disToEnemy <= attackDis)
            {
                flag = true;
                Managers.Game.CanMove = false;
                Attack(enemy);
                break;
            }
        }

        if (!flag)
            Managers.Game.CanMove = true;
    }

    private void Attack(GameObject enemy)
    {
        
    }
}
