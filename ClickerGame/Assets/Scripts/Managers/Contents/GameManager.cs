using UnityEngine;

public class GameManager
{
    public MyPlayerController MyPlayer { get; set; }
    public GameObject CurMap { get; set; }
    public GameObject PreMap { get; set; }
    public Wave Wave { get; set; }

    // �����ͻӸ� �ƴ϶� ��� �غ�(���� �ʰ� �Ǵ� HUD����)�� �Ϸ� �Ǹ� True
    public bool GameStartReady { get; set; } = false;
}
