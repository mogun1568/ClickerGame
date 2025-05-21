using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UI_Base
{
    public enum InfoType { Coin, Round, HP }
    [SerializeField]
    InfoType _type;

    Slider _slider;
    TextMeshProUGUI _Text;

    private void Start()
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
            case InfoType.Round:
                int MyPlayerRound = Managers.Data.MyPlayerInfo.Round;
                _Text.text = $"{MyPlayerRound} ¶ó¿îµå";
                break;
            case InfoType.HP:
                float curHP = Managers.Game.MyPlayer.HP;
                float maxHP = Managers.Game.MyPlayer.MaxHP;
                _slider.value = curHP / maxHP;
                break;
        }
    }
}
