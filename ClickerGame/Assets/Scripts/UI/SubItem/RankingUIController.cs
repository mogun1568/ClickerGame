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

    private void Awake()
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
        await UniTask.WaitUntil(() => Managers.Ranking._updateDone);
        RankObjectManagement();
    }

    private async UniTask RankingRefresh()
    {
        Managers.Ranking._updateDone = false;
        await InitAsync();
    }

    private void RankObjectManagement()
    {
        int currentCount = _content.childCount;

        // Ȥ�� ���� �ִ� 10���� ����
        int targetCount = Mathf.Min(Managers.Ranking.RankingList.Count, 10);

        if (currentCount < targetCount)
        {
            int toCreate = targetCount - currentCount;
            for (int i = 0; i < toCreate; i++)
            {
                Managers.Resource.Instantiate("UI/SubItem/Rank", default, _content);
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
