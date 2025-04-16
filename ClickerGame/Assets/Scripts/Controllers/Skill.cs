using DG.Tweening;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private int _knockback;
    private int _slow;

    private CreatureController Controller;
    private Tween KnockbackTween;

    private void Start()
    {
        Controller = GetComponent<CreatureController>();
    }

    void Knockback()
    {
        //if (KnockbackTween != null)
        //    KnockbackTween.Kill();

        //float duration = Mathf.Abs(transform.position.x - endPosX) / moveSpeed;

        //KnockbackTween = transform.DOMoveX(endPosX, duration)
        //    .SetEase(Ease.Linear)
        //    .SetAutoKill(true)
        //    .OnKill(() => KnockbackTween = null)
        //    .OnComplete(() =>
        //    {
        //        // 이동 완료 시 호출
        //        Controller.Move(Controller._endPosX, Controller._moveSpeed);    
        //    });
    }

    void Slow(float endPosX, float moveSpeed)
    {
        
    }

    private void SkillComplete()
    {

    }
}
