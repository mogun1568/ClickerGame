using System.Collections.Generic;
using System;

namespace Data
{
    // Info�� Data.Contents.Info�� ����
    // Stat�̶� Skill�̶� class ����ؼ� ��ĥ�� ���

    #region Stat
    [Serializable]
    public class StatInfo
    {
        //public string statType;
        public int statLevel;
        public float statValue;
        public int statPrice;
    }
    #endregion

    #region SkillInfo
    [Serializable]
    public class SkillInfo
    {
        //public string skillType;
        public int skillLevel;
        public float skillValue;
    }
    #endregion

    #region Game
    [Serializable]
    public class GameData
    {
        public Info info = new Info();
        public Dictionary<string, StatInfo> stats = new Dictionary<string, StatInfo>();
        public Dictionary<string, SkillInfo> skills = new Dictionary<string, SkillInfo>();
    }
    #endregion

    #region Ranking
    public class RankingData
    {
        public string userId;
        public string nickname;
        public int reincarnation;
        public int round;
    }
    #endregion
}