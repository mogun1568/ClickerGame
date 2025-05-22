using TMPro;
using UnityEngine.UI;

public class UI_Rank : UI_Base
{
    enum Images
    {
        Icon_Player
    }

    enum Texts
    {
        Text_RankNumber,
        Text_Nickname,
        Text_ReincarnationRank,
        Text_RoundRank
    }

    //Data.RankingData _rank;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        int idx = transform.GetSiblingIndex() + 1;
        GetText((int)Texts.Text_RankNumber).text = idx.ToString();
    }

    public void LoadRank(Data.RankingData rank)
    {
        //_rank = rank;

        //Image icon = GetImage((int)Images.Icon_Player);
        //icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{}");
        
        GetText((int)Texts.Text_Nickname).text = rank.nickname;
        GetText((int)Texts.Text_ReincarnationRank).text = "환생: " + rank.reincarnation;
        GetText((int)Texts.Text_RoundRank).text = "라운드: " + rank.round;
    }
}
