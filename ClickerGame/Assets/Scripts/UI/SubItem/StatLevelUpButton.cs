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
            case "HP":
                Managers.Game.MyPlayer.Stat.HP += Managers.Game.MyPlayer.Stat.IncreaseHP;
                Debug.Log($"HP -> {Managers.Game.MyPlayer.Stat.HP}");
                break;
            case "ATK":
                Managers.Game.MyPlayer.Stat.ATK += Managers.Game.MyPlayer.Stat.IncreaseATK;
                Debug.Log($"ATK -> {Managers.Game.MyPlayer.Stat.ATK}");
                break;
            case "DEF":
                Managers.Game.MyPlayer.Stat.DEF += Managers.Game.MyPlayer.Stat.IncreaseDEF;
                Debug.Log($"DEF -> {Managers.Game.MyPlayer.Stat.DEF}");
                break;
            case "AttackSpeed":
                Managers.Game.MyPlayer.Stat.AttackSpeed += Managers.Game.MyPlayer.Stat.IncreaseAttackSpeed;
                Debug.Log($"AttackSpeed -> {Managers.Game.MyPlayer.Stat.AttackSpeed}");
                break;
            case "Range":
                Managers.Game.MyPlayer.Stat.Range += Managers.Game.MyPlayer.Stat.IncreaseRange;
                Debug.Log($"Range -> {Managers.Game.MyPlayer.Stat.Range}");
                break;
        }

        Managers.Game.MyPlayer.UpdateDict();
    }

    public override void Init()
    {
        
    }
}
