
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : Stat
{
    public EnemyStat(Data.Enemy enemyStat)
    {
        MaxHP = enemyStat.enemyMaxHP;
        HP = enemyStat.enemyMaxHP;
        ATK = enemyStat.enemyATK;
        DEF = enemyStat.enemyDEF;
        AttackSpeed = enemyStat.enemyAttackSpeed;
        Range = enemyStat.enemyRange;
        Coin = enemyStat.enemyCoin;
    }
}
