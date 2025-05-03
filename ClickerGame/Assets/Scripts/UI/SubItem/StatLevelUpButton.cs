using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatLevelUpButton : UI_Base
{
    enum Buttons
    {
        LevelUp_Button
    }

    enum Images
    {
        Icon_Stat
    }

    enum Texts
    {
        Text_StatLevel,
        Text_StatName,
        Text_StatValue,
        Text_StatIncreaseValue,
        Text_StatPrice
    }
    
    private Dictionary<string, Data.StatInfo> _myPlayerStatDict;
    private Dictionary<string, AbilityData> _statDict;
    string _statName;

    private float _statIncreaseValue;
    private int _statIncreasePrice;

    private void Start()
    {
        Init();
        DataInitAsync().Forget();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.LevelUp_Button).gameObject, (PointerEventData data) => { StatUpdate(); }, Define.UIEvent.Click); 
    }

    private async UniTask DataInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        _myPlayerStatDict = Managers.Data.MyPlayerStatDict;
        _statDict = Managers.Resource.StatDict;
        _statName = gameObject.name;

        _statIncreaseValue = _statDict[_statName].abilityIncreaseValue;
        _statIncreasePrice = _statDict[_statName].statIncreasePrice;

        //Image icon = GetImage((int)Images.Icon_Stat);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");
        GetText((int)Texts.Text_StatName).text = _statDict[_statName].abilityName.ToString();
        GetText((int)Texts.Text_StatIncreaseValue).text = "+" + _statIncreaseValue.ToString();
        HUDUpdate();
    }

    private void StatUpdate()
    {
        if (!CheckCoin())
            return;

        DetailStatUpdate();

        switch (_statName)
        {
            case "MaxHP":
                Managers.Game.MyPlayer.MaxHP += _statIncreaseValue;
                //Debug.Log($"MaxHP -> {Managers.Game.MyPlayer.MaxHP}");
                //Debug.Log($"HP -> {Managers.Game.MyPlayer.HP}");
                break;
            case "Regeneration":
                Managers.Game.MyPlayer.Regeneration += _statIncreaseValue;
                //Debug.Log($"Regeneration -> {Managers.Game.MyPlayer.Regeneration}");
                break;
            case "ATK":
                Managers.Game.MyPlayer.StatInfo.ATK += _statIncreaseValue;
                //Debug.Log($"ATK -> {Managers.Game.MyPlayer.StatInfo.ATK}");
                break;
            case "DEF":
                Managers.Game.MyPlayer.StatInfo.DEF += _statIncreaseValue;
                //Debug.Log($"DEF -> {Managers.Game.MyPlayer.StatInfo.DEF}");
                break;
            case "AttackSpeed":
                Managers.Game.MyPlayer.AttackSpeed += _statIncreaseValue;
                //Debug.Log($"AttackSpeed -> {Managers.Game.MyPlayer.AttackSpeed}");
                break;
            case "Range":
                Managers.Game.MyPlayer.StatInfo.Range += _statIncreaseValue;
                //Debug.Log($"Range -> {Managers.Game.MyPlayer.StatInfo.Range}");
                break;
        }

        HUDUpdate();
    }

    private bool CheckCoin()
    {
        if (_statDict[_statName].statPrice > Managers.Game.MyPlayer.StatInfo.Coin)
            return false;

        Managers.Game.MyPlayer.StatInfo.Coin -= _statDict[_statName].statPrice;

        return true;
    }

    private void DetailStatUpdate()
    {
        _myPlayerStatDict[_statName].statLevel++;
        _myPlayerStatDict[_statName].statPrice += _statIncreasePrice;
    }

    private void HUDUpdate()
    {
        GetText((int)Texts.Text_StatLevel).text = _myPlayerStatDict[_statName].statLevel.ToString();
        GetText((int)Texts.Text_StatValue).text = _myPlayerStatDict[_statName].statValue.ToString();
        GetText((int)Texts.Text_StatPrice).text = _myPlayerStatDict[_statName].statPrice.ToString();
    }
}
