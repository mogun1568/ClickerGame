using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankingUIController : UI_Base
{
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        Button_Refresh
    }

    private Transform _content;

    void Awake()
    {
        Init();
        InitAsync().Forget();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        _content = GetObject((int)GameObjects.Content).transform;
        BindEvent(GetButton((int)Buttons.Button_Refresh).gameObject, (PointerEventData data) => { RankingRefresh().Forget(); }, Define.UIEvent.Click);
    }

    private async UniTask InitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);
        await RankingRefresh();
    }

    private async UniTask RankingRefresh()
    {
        await Managers.Ranking.InitAsync();
        RankObjectManagement();
    }

    private void RankObjectManagement()
    {
        int currentCount = _content.childCount;

        // 혹시 몰라 최대 10개로 제한
        int targetCount = Mathf.Min(Managers.Ranking.RankingList.Count, 10);

        if (currentCount < targetCount)
        {
            GameObject go;
            int toCreate = targetCount - currentCount;
            for (int i = 0; i < toCreate; i++)
            {
                go = Managers.Resource.Instantiate("UI/SubItem/Rank", default, _content);
                go.GetComponent<UI_Rank>().Init();
            }
        }
        else if (currentCount > targetCount)
        {
            int toRemove = currentCount - targetCount;
            for (int i = 0; i < toRemove; i++)
            {
                Managers.Resource.Destroy(_content.GetChild(_content.childCount - 1 - i).gameObject);
            }
        }

        for (int i = 0; i < targetCount; i++)
        {
            UI_Rank rank = _content.GetChild(i).GetComponent<UI_Rank>();

            if (rank != null)
            {
                rank.LoadRank(Managers.Ranking.RankingList[i]);
            }
        }
    }
}
