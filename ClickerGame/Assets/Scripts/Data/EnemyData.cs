using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public Define.EnemyType enemyType;
    public string enemyName;
    public float enemyMaxHP;
    public float enemyATK;
    public float enemyDEF;
    public float enemyAttackSpeed;
    public float enemyRange;
    public float enemyStaggerResistance;
    public float enemyMoveSpeed;
    public int enemyCoin;

    public EnemyData Copy()
    {
        EnemyData copied = CreateInstance<EnemyData>();
        copied.enemyType = this.enemyType;
        copied.enemyName = this.enemyName;
        copied.enemyMaxHP = this.enemyMaxHP;
        copied.enemyATK = this.enemyATK;
        copied.enemyDEF = this.enemyDEF;
        copied.enemyAttackSpeed = this.enemyAttackSpeed;
        copied.enemyRange = this.enemyRange;
        copied.enemyStaggerResistance = this.enemyStaggerResistance;
        copied.enemyMoveSpeed = this.enemyMoveSpeed;
        copied.enemyCoin = this.enemyCoin;

        return copied;
    }
}
