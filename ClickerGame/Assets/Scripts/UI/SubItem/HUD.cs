using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UI_Base
{
    public enum InfoType { Coin, HP }
    [SerializeField]
    InfoType _type;

    Slider _slider;
    TextMeshProUGUI _Text;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _Text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        switch (_type)
        {
            case InfoType.Coin:
                int MyPlayerCoin = Managers.Game.MyPlayer.StatInfo.Coin;
                _Text.text = MyPlayerCoin.ToString("N0");
                break;
            case InfoType.HP:
                float curHP = Managers.Game.MyPlayer.StatInfo.HP;
                float maxHP = Managers.Game.MyPlayer.StatInfo.MaxHP;
                _slider.value = curHP / maxHP;
                break;
        }
    }

    public override void Init()
    {
        
    }
}
