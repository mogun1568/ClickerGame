using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Bgm,
        Effect,
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
        Hurt,
        Death
    }
}