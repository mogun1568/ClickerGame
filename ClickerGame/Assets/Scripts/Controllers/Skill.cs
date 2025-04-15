using DG.Tweening;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private CreatureController Controller;
    private Tween KnockbackTween;

    private void Start()
    {
        Controller = GetComponent<CreatureController>();
    }

    void Knockback(float endPosX, float moveSpeed)
    {
        
    }

    void Slow(float endPosX, float moveSpeed)
    {
        
    }

    private void SkillComplete()
    {

    }
}
