public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        GamePlay,
    }

    public enum Sound
    {
        BGM,
        SFX,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
        BeginDrag,
        EndDrag,
    }

    public enum CameraMode
    {
        QuarterView,
    }

    public enum State
    {
        None,
        Idle,
        Run,
        Attack,
        Stagger,    // °æÁ÷
        Death
    }

    public enum AbilityType
    {
        Stat,
        Skill
    }

    public enum ClassType
    {
        Knight,
        Wizard
    }

    public enum EnemyType
    {
        General,
        Boss
    }

    public enum ShopItemType
    {
        Common,
        Skin,
        Class
    }

    public enum Debuff
    {
        None,
        Slow
    }

    public enum TweenType
    {
        None,
        Idle,
        Run,
        Knockback,
        Slow
    }

    public enum RewardAdType
    {
        Respawn,
        Offline,
        GiveUp,
        AddCoin
    }
}