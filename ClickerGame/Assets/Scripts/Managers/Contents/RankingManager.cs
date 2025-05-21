using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class RankingManager
{
    List<Data.RankingData> RankingList;

    public async UniTask InitAsync()
    {
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
    }
}