
public class EnemyStat : Stat
{
    private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public EnemyStat(Data.Enemy enemyStat)
    {
        MaxHP = enemyStat.enemyMaxHP;
        HP = enemyStat.enemyMaxHP;
        ATK = enemyStat.enemyATK;
        DEF = enemyStat.enemyDEF;
        AttackSpeed = enemyStat.enemyAttackSpeed;
        Range = enemyStat.enemyRange;
        MoveSpeed = enemyStat.enemyMoveSpeed;
        Coin = enemyStat.enemyCoin;
    }
}
