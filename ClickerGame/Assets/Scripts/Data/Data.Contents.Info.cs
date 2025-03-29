using System;

namespace Data
{
    [Serializable]
    public class Info
    {
        private int _coin;
        private float _hp;
        private long _lastTime;
        private int _offlineReward;

        public int Coin
        {
            get => _coin;
            set => SetValue(ref _coin, value, nameof(Coin));
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
                Managers.Data.UpdateInfo(key, newValue);
            }
        }
    }
}

