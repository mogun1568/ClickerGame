using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Dictionary<string, Data.SkillInfo> MyPlayerSkillDict;

    public virtual void Init()
    {
        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
    }

    public virtual void Knockback(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();

        float endPosX = target.transform.position.x + MyPlayerSkillDict["Knockback"].skillValue;
        float moveSpeed = 10f;
        controller.Move(endPosX, moveSpeed, Define.TweenType.Knockback);
    }

    public virtual void Slow(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();
        controller.StartCoroutine(SlowedCoroutine(controller));
    }

    // Slow걸린 대상의 스크립트에서 실행되는 함수, 매개변수로 스크립트를 주는게 이상할 수도
    public IEnumerator SlowedCoroutine(CreatureController controller)
    {
        float _preMoveSpeed = controller._moveSpeed;
        controller._moveSpeed *= MyPlayerSkillDict["Slow"].skillValue;
        controller._debuff = Define.Debuff.Slow;
        controller.Move(controller._endPosX, controller._moveSpeed, Define.TweenType.Slow);

        yield return new WaitForSeconds(5);

        controller._moveSpeed = _preMoveSpeed;
        controller._debuff = Define.Debuff.None;
        controller.Move(controller._endPosX, controller._moveSpeed, Define.TweenType.Run);
    }
}
