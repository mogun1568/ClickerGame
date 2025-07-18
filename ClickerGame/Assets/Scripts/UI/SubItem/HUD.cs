using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UI_Base
{
    public enum InfoType { Coin, Reincarnation, Round, HP }
    [SerializeField]
    InfoType _type;

    Slider _slider;
    TextMeshProUGUI _Text;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        _slider = GetComponent<Slider>();
        _Text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        if (!Managers.Data.GameDataReady)
            return;

        if (Managers.Game.MyPlayer == null)
            return;

        switch (_type)
        {
            case InfoType.Coin:
                int MyPlayerCoin = Managers.Data.MyPlayerInfo.Coin;
                _Text.text = MyPlayerCoin.ToString("N0");
                break;
            case InfoType.Reincarnation:
                int MyPlayerReincarnation = Managers.Data.MyPlayerInfo.Reincarnation;
                _Text.text = $"환생: {MyPlayerReincarnation}";
                break;
            case InfoType.Round:
                int MyPlayerRound = Managers.Data.MyPlayerInfo.Round;
                _Text.text = $"라운드: {MyPlayerRound}";
                break;
            case InfoType.HP:
                float curHP = Managers.Game.MyPlayer.HP;
                float maxHP = Managers.Game.MyPlayer.MaxHP;
                _slider.value = curHP / maxHP;
                break;
        }

        if (!Managers.Game.GameStartReady)
            Managers.Game.GameStartReady = true;
    }
}
