using UnityEngine;

public class EnemyStat : Stat
{
    private float _moveSpeed;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public EnemyStat(string enemyName)
    {
        EnemyData enemyInfo = Managers.Resource.EnemyDict[enemyName];
        float reincarnationScale = 0f;
        float roundScale = 0f;

        if (enemyInfo.enemyType == Define.EnemyType.General)
        {
            reincarnationScale = Managers.Data.MyPlayerInfo.Reincarnation * 0.1f;
            roundScale = (Managers.Data.MyPlayerInfo.Round - 1) * 0.05f;

            Coin = enemyInfo.enemyCoin * Mathf.Max(Managers.Data.MyPlayerInfo.Round / 10, 1);
        }
        else
        {
            Coin = enemyInfo.enemyCoin * Mathf.Max(Managers.Data.MyPlayerInfo.Round / 50, 1);
        }

        float scale = 1f + reincarnationScale + roundScale;

        MaxHP = enemyInfo.enemyMaxHP * scale;
        HP = enemyInfo.enemyMaxHP * scale;
        ATK = enemyInfo.enemyATK * scale;
        DEF = enemyInfo.enemyDEF * scale;
        AttackSpeed = enemyInfo.enemyAttackSpeed;
        Range = enemyInfo.enemyRange;
        StaggerResistance = enemyInfo.enemyStaggerResistance * scale;
        MoveSpeed = enemyInfo.enemyMoveSpeed;

        //Logging.Log($"[EnemyStat: {enemyName}]" +
        //      $"\nHP: {HP} / MaxHP: {MaxHP}" +
        //      $"\nATK: {ATK}" +
        //      $"\nDEF: {DEF}" +
        //      $"\nAttackSpeed: {AttackSpeed}" +
        //      $"\nRange: {Range}" +
        //      $"\nStaggerResistance: {StaggerResistance}" +
        //      $"\nMoveSpeed: {MoveSpeed}" +
        //      $"\nCoin: {Coin}" +
        //      $"\nScale: {scale} (Reincarnation: {reincarnationScale}, Round: {roundScale})");
    }
}
