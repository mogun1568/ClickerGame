using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class RankingManager
{
    public List<Data.RankingData> RankingList = new List<Data.RankingData>();
    public bool _updateDone;

    public async UniTask InitAsync()
    {
        _updateDone = false;
        await UpdateRankingAsync();
    }

    public async UniTask UpdateRankingAsync()
    {
        RankingList = await Managers.Data.LoadRanking();
        SortRankingList();
    }

    private void SortRankingList()
    {
        RankingList.Sort((a, b) =>
        {
            // Reincarnation ���� ��
            int compareReincarnation = b.reincarnation.CompareTo(a.reincarnation);
            if (compareReincarnation != 0)
                return compareReincarnation;

            // Reincarnation�� ���ٸ� Round ���� ��
            return b.round.CompareTo(a.round);
        });

        _updateDone = true;
    }
}