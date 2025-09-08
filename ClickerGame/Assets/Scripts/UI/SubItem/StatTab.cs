using UnityEngine;

public class StatTab : UI_Base
{
    enum GameObjects
    {
        Content
    }

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        StatInit();
    }

    private void StatInit()
    {
        foreach (var pair in Managers.Resource.StatList)
        {
            GameObject stat = Managers.Resource.Instantiate("UI/SubItem/Stat", default, GetObject((int)GameObjects.Content).transform);
            stat.name = pair.abilityKind;
            stat.GetComponent<UI_Stat>().Init();
        }
    }
}
