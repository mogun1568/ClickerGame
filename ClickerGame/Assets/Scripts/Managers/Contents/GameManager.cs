using UnityEngine;

public class GameManager
{
    public MyPlayerController MyPlayer { get; set; }
    public GameObject CurMap { get; set; }
    public GameObject PreMap { get; set; }
    public Wave Wave { get; set; }

    // 데이터뿐만 아니라 모든 준비(가장 늦게 되는 HUD까지)가 완료 되면 True
    public bool GameStartReady { get; set; } = false;
}
