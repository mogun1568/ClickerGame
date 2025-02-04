using TMPro;
using UnityEngine;

public class HUD : UI_Base
{
    public enum InfoType { Coin }
    [SerializeField]
    InfoType _type;

    TextMeshProUGUI _Text;

    void Awake()
    {
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
        }
    }

    public override void Init()
    {
        
    }
}
