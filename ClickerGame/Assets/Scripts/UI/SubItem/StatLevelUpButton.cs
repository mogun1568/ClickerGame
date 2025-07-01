using Cysharp.Threading.Tasks;
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
        Text_StatPrice,
        Text_StatMaxLevel
    }
    
    private Dictionary<string, Data.StatInfo> _myPlayerStatDict;
    private Dictionary<string, AbilityData> _statDict;
    string _statName;

    private float _statIncreaseValue;
    private int _statIncreasePrice;
    private int _statMaxLevel;

    // Start
    void Awake()
    {
        Init();
        DataInitAsync().Forget();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.LevelUp_Button).gameObject, (PointerEventData data) => { StatUpdate(); }, Define.UIEvent.Click, false);

        _statName = gameObject.name;
        _statDict = Managers.Resource.StatDict;

        _statIncreaseValue = _statDict[_statName].abilityIncreaseValue;
        _statIncreasePrice = _statDict[_statName].statIncreasePrice;
        _statMaxLevel = _statDict[_statName].abilityMaxLevel;

        Image icon = GetImage((int)Images.Icon_Stat);
        icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{_statDict[_statName].abilityIcon}");
        GetText((int)Texts.Text_StatName).text = _statDict[_statName].abilityName.ToString();
        GetText((int)Texts.Text_StatIncreaseValue).text = "+" + _statIncreaseValue.ToString();
        GetText((int)Texts.Text_StatMaxLevel).text = "최대 레벨\n" + _statMaxLevel.ToString();
    }

    private async UniTask DataInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);
        _myPlayerStatDict = Managers.Data.MyPlayerStatDict;
        HUDUpdate();
    }

    private void StatUpdate()
    {
        if (!CheckLevel())
            return;

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

        Managers.Sound.Play("SFX_Powerup_Bright_1", Define.Sound.SFX);
    }

    private bool CheckCoin()
    {
        if (_myPlayerStatDict[_statName].statPrice > Managers.Game.MyPlayer.StatInfo.Coin)
        {
            Managers.UI.ToastMessage.Show("골드가 부족합니다.");
            return false;
        }    

        Managers.Game.MyPlayer.StatInfo.Coin -= _myPlayerStatDict[_statName].statPrice;

        return true;
    }

    private bool CheckLevel()
    {
        if (_myPlayerStatDict[_statName].statLevel >= _statDict[_statName].abilityMaxLevel)
        {
            Managers.UI.ToastMessage.Show("최대 레벨입니다.");
            return false;
        }

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

        if (_myPlayerStatDict[_statName].statLevel < _statDict[_statName].abilityMaxLevel)
            GetText((int)Texts.Text_StatPrice).text = _myPlayerStatDict[_statName].statPrice.ToString();
        else
            GetText((int)Texts.Text_StatPrice).text = "최대 레벨";
    }
}
