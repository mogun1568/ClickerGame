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

    public enum EnemyType
    {
        General
    }

    public enum Debuff
    {
        None,
        Slow
    }

    public enum TweenType
    {
        Idle,
        Run,
        Knockback,
        Slow
    }
}