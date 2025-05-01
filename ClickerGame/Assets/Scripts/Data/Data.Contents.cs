using System.Collections.Generic;
using System;

namespace Data
{
    // Info는 Data.Contents.Info에 정리

    #region Stat
    [Serializable]
    public class StatInfo
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

    #region SkillInfo
    [Serializable]
    public class SkillInfo
    {
        public string skillType;
        public string skillIcon;
        public int skillLevel;
        public string skillName;
        public float skillValue;
        public float skillIncreaseValue;
        public float skillCoolTime;

        public SkillInfo()
        {
        }

        public SkillInfo(SkillInfo other)
        {
            skillType = other.skillType;
            skillIcon = other.skillIcon;
            skillLevel = other.skillLevel;
            skillName = other.skillName;
            skillValue = other.skillValue;
            skillIncreaseValue = other.skillIncreaseValue;
            skillCoolTime = other.skillCoolTime;
        }
    }
    #endregion

    #region Enemy
    [Serializable]
    public class EnemyInfo
    {
        public string enemyType;
        public string enemyName;
        public float enemyMaxHP;
        public float enemyATK;
        public float enemyDEF;
        public float enemyAttackSpeed;
        public float enemyRange;
        public float enemyMoveSpeed;
        public int enemyCoin;
    }
    #endregion

    #region Game
    [Serializable]
    public class GameData
    {
        public Info info = new Info();
        public Dictionary<string, StatInfo> stats = new Dictionary<string, StatInfo>();
        public Dictionary<string, SkillInfo> skills = new Dictionary<string, SkillInfo>();
        public Dictionary<string, EnemyInfo> enemys = new Dictionary<string, EnemyInfo>();
    }
    #endregion
}