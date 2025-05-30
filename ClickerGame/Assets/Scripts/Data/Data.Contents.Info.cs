using System;

namespace Data
{
    [Serializable]
    public class Info
    {
        private string _nickname;
        private int _reincarnation; // È¯»ý
        private int _coin;
        private int _round;
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

        public int Coin
        {
            get => _coin;
            set => SetValue(ref _coin, value, nameof(Coin));
        }

        public int Round
        {
            get => _round;
            set => SetValue(ref _round, value, nameof(Round));
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
