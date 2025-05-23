
public class EnemyStat : Stat
{
    private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }


    // 현재는 적의 밸런스를 결정하지 못해 dic이랑 ScriptableObject 중에 뭐를 쓸 지 결정 못함
    public EnemyStat(Data.EnemyInfo enemyStat)
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
