using System.Collections.Generic;
using System;
using UnityEngine;

namespace Data
{
    #region Info
    [Serializable]
    public class Info
    {
        public int coin;
        public float HP;
        public long lastTime;
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
        public List<Stat> stats = new List<Stat>();
        public List<Enemy> enemys = new List<Enemy>();

        public Dictionary<string, T> MakeDict<T>(List<T> dataList, Func<T, string> keySelector)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (var data in dataList)
            {
                dict[keySelector(data)] = data;
            }
            return dict;
        }
    }
    #endregion
}