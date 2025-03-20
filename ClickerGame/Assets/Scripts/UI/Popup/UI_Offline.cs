using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Offline : UI_Popup
{
    enum Buttons
    {
        Button_Collect,
        Button_x2Collect,
    }

    enum Images
    {
        Icon_Gold
    }

    enum Texts
    {
        Text_Gold
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_Collect).gameObject, (PointerEventData data) => { GetReward(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_x2Collect).gameObject, (PointerEventData data) => { GetReward(2); }, Define.UIEvent.Click);

        //Image icon = GetImage((int)Images.Icon_Gold);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");
        //GetText((int)Texts.Text_Gold).text = ;
    }

    private int _rewardCoin;

    public void StatInit(int coin)
    {
        _rewardCoin = coin;
        GetText((int)Texts.Text_Gold).text = _rewardCoin.ToString();
    }

    private void GetReward(int amount = 1)
    {
        Managers.Game.MyPlayer.StatInfo.Coin += _rewardCoin * amount;
        ClosePopupUI();
    }
}
