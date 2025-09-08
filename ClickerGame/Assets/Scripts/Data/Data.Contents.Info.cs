using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Info
    {
        private string _nickname;
        private int _reincarnation; // È¯»ý
        private int _round;
        private int _coin;
        private string _map;
        private string _class;
        private string _skin;
        private float _hp;
        private long _lastTime;
        private int _offlineReward;

        public string Nickname
        {
            get => _nickname;
            set => SetValue(ref _nickname, value, nameof(Nickname));
        }

        public int Reincarnation
        {
            get => _reincarnation;
            set => SetValue(ref _reincarnation, value, nameof(Reincarnation));
        }

        public int Round
        {
            get => _round;
            set => SetValue(ref _round, value, nameof(Round));
        }

        public int Coin
        {
            get => _coin;
            set
            {
                value = Mathf.Max(value, 0);
                SetValue(ref _coin, value, nameof(Coin));
            }
        }

        public string Map
        {
            get => _map;
            set => SetValue(ref _map, value, nameof(Map));
        }
        
        public string Class
        {
            get => _class;
            set => SetValue(ref _class, value, nameof(Class));
        }

        public string Skin
        {
            get => _skin;
            set => SetValue(ref _skin, value, nameof(Skin));
        }

        public float HP
        {
            get => _hp;
            set => SetValue(ref _hp, value, nameof(HP));
        }

        public long LastTime
        {
            get => _lastTime;
            set => SetValue(ref _lastTime, value, nameof(LastTime));
        }

        public int OfflineReward
        {
            get => _offlineReward;
            set => SetValue(ref _offlineReward, value, nameof(OfflineReward));
        }

        private void SetValue<T>(ref T field, T newValue, string key)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;

                if (Managers.Data.GameDataReady)
                    Managers.Data.UpdateInfo(key, newValue);
            }
        }
    }
}
