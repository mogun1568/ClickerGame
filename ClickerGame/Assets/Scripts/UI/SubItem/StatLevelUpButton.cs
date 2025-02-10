using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    
    private Dictionary<string, Data.Stat> _statDict;
    string _statName;

    private float _statIncreaseValue;
    private int _statIncreasePrice;


    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.LevelUp_Button).gameObject, (PointerEventData data) => { StatUpdate(); }, Define.UIEvent.Click);

        _statDict = Managers.Data.MyPlayerStatDict;
        _statName = gameObject.name;

        _statIncreaseValue = _statDict[_statName].statIncreaseValue;
        _statIncreasePrice = _statDict[_statName].statIncreasePrice;

        //Image icon = GetImage((int)Images.Icon_Stat);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");
        GetText((int)Texts.Text_StatName).text = _statDict[_statName].statName.ToString();
        GetText((int)Texts.Text_StatIncreaseValue).text = "+" + _statDict[_statName].statIncreaseValue.ToString();
        HUDUpdate();
    }

    private void StatUpdate()
    {
        if (!CheckCoin())
            return;

        switch (_statName)
        {
            case "MaxHP":
                Managers.Game.MyPlayer.MaxHP += _statIncreaseValue;
                Debug.Log($"MaxHP -> {Managers.Game.MyPlayer.MaxHP}");
                Debug.Log($"HP -> {Managers.Game.MyPlayer.HP}");
                break;
            case "Regeneration":
                Managers.Game.MyPlayer.Regeneration += _statIncreaseValue;
                Debug.Log($"Regeneration -> {Managers.Game.MyPlayer.Regeneration}");
                break;
            case "ATK":
                Managers.Game.MyPlayer.StatInfo.ATK += _statIncreaseValue;
                Debug.Log($"ATK -> {Managers.Game.MyPlayer.StatInfo.ATK}");
                break;
            case "DEF":
                Managers.Game.MyPlayer.StatInfo.DEF += _statIncreaseValue;
                Debug.Log($"DEF -> {Managers.Game.MyPlayer.StatInfo.DEF}");
                break;
            case "AttackSpeed":
                Managers.Game.MyPlayer.AttackSpeed += _statIncreaseValue;
                Debug.Log($"AttackSpeed -> {Managers.Game.MyPlayer.AttackSpeed}");
                break;
            case "Range":
                //Managers.Game.MyPlayer.StatInfo.Range += _statIncreaseValue;
                Managers.Game.MyPlayer.StatInfo.Range = Mathf.Round((Managers.Game.MyPlayer.StatInfo.Range + _statIncreaseValue) * 100f) / 100f;
                Debug.Log($"Range -> {Managers.Game.MyPlayer.StatInfo.Range}");
                break;
        }

        Managers.Game.MyPlayer.UpdateDict();
        DetailStatUpdate();
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
        _statDict[_statName].statLevel++;
        _statDict[_statName].statPrice += _statIncreasePrice;

        HUDUpdate();
    }

    private void HUDUpdate()
    {
        GetText((int)Texts.Text_StatLevel).text = _statDict[_statName].statLevel.ToString();
        GetText((int)Texts.Text_StatValue).text = _statDict[_statName].statValue.ToString();
        GetText((int)Texts.Text_StatPrice).text = _statDict[_statName].statPrice.ToString();
    }

    public override void Init()
    {
        
    }
}
