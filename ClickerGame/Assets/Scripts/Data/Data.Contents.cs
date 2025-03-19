using System.Collections.Generic;
using System;

namespace Data
{
    #region Info
    [Serializable]
    public class Info
    {
        public int Coin;
        public float HP;
        public long LastTime;
    }
    #endregion

    #region Stat
    [Serializable]
    public class Stat
    {
        public string statType;
        public string statIcon;
        public int statLevel;
        public string statName;
        public float statValue;
        public float statIncreaseValue;
        public int statPrice;
        public int statIncreasePrice;
    }
    #endregion

    #region Enemy
    [Serializable]
    public class Enemy
    {
        public string enemyType;
        public string enemyName;
        public float enemyMaxHP;
        public float enemyATK;
        public float enemyDEF;
        public float enemyAttackSpeed;
        public float enemyRange;
        public int enemyCoin;
    }
    #endregion

    #region Game
    [Serializable]
    public class GameData
    {
        public Info info = new Info();
        public Dictionary<string, Stat> stats = new Dictionary<string, Stat>();
        public Dictionary<string, Enemy> enemys = new Dictionary<string, Enemy>();
    }
    #endregion
}