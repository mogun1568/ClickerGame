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

        float reincarnationScale = Managers.Data.MyPlayerInfo.Reincarnation * 0.1f;
        float roundScale = (Managers.Data.MyPlayerInfo.Round - 1) * 0.05f;
        float scale = 1f + reincarnationScale + roundScale;

        MaxHP = enemyInfo.enemyMaxHP * scale;
        HP = enemyInfo.enemyMaxHP * scale;
        ATK = enemyInfo.enemyATK * scale;
        DEF = enemyInfo.enemyDEF * scale;
        AttackSpeed = enemyInfo.enemyAttackSpeed;
        Range = enemyInfo.enemyRange;
        StaggerResistance = enemyInfo.enemyStaggerResistance * scale;
        MoveSpeed = enemyInfo.enemyMoveSpeed;
        Coin = enemyInfo.enemyCoin * Mathf.Max(Managers.Data.MyPlayerInfo.Round / 10, 1);

        //Debug.Log($"[EnemyStat: {enemyName}]" +
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
