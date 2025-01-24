using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatLevelUpButton : UI_Base
{
    enum Buttons
    {
        LevelUp_Button
    }

    string _statName;

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.LevelUp_Button).gameObject, (PointerEventData data) => { StatUpdate(); }, Define.UIEvent.Click);

        _statName = gameObject.name;
    }


    private void StatUpdate()
    {
        switch (_statName)
        {
            case "MaxHP":
                Managers.Game.MyPlayer.StatInfo.MaxHP += Managers.Game.MyPlayer.StatInfo.IncreaseMaxHP;
                Managers.Game.MyPlayer.StatInfo.HP += Managers.Game.MyPlayer.StatInfo.IncreaseMaxHP;
                Managers.Game.MyPlayer.StatInfo.HP = Mathf.Min(Managers.Game.MyPlayer.StatInfo.MaxHP, Managers.Game.MyPlayer.StatInfo.HP);
                Debug.Log($"MaxHP -> {Managers.Game.MyPlayer.StatInfo.MaxHP}");
                Debug.Log($"HP -> {Managers.Game.MyPlayer.StatInfo.HP}");
                break;
            case "ATK":
                Managers.Game.MyPlayer.StatInfo.ATK += Managers.Game.MyPlayer.StatInfo.IncreaseATK;
                Debug.Log($"ATK -> {Managers.Game.MyPlayer.StatInfo.ATK}");
                break;
            case "DEF":
                Managers.Game.MyPlayer.StatInfo.DEF += Managers.Game.MyPlayer.StatInfo.IncreaseDEF;
                Debug.Log($"DEF -> {Managers.Game.MyPlayer.StatInfo.DEF}");
                break;
            case "AttackSpeed":
                Managers.Game.MyPlayer.AttackSpeed += Managers.Game.MyPlayer.StatInfo.IncreaseAttackSpeed;
                Debug.Log($"AttackSpeed -> {Managers.Game.MyPlayer.AttackSpeed}");
                break;
            case "Range":
                Managers.Game.MyPlayer.StatInfo.Range += Managers.Game.MyPlayer.StatInfo.IncreaseRange;
                Debug.Log($"Range -> {Managers.Game.MyPlayer.StatInfo.Range}");
                break;
        }

        Managers.Game.MyPlayer.UpdateDict();
    }

    public override void Init()
    {
        
    }
}
