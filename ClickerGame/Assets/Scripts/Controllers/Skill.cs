using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Dictionary<string, Data.Skill> MyPlayerSkillDict;

    public virtual void Init()
    {
        MyPlayerSkillDict = Managers.Data.MyPlayerSkillDict;
    }

    public virtual void Knockback(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();

        float endPosX = target.transform.position.x + MyPlayerSkillDict["Knockback"].skillValue;
        float moveSpeed = 5f;
        controller.Move(endPosX, moveSpeed);
    }

    public virtual void Slow(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();

        float preMoveSpeed = controller._moveSpeed;
        controller._moveSpeed *= MyPlayerSkillDict["Slow"].skillValue;
        StartCoroutine(SlowCoroutine(controller, preMoveSpeed));
    }

    protected IEnumerator SlowCoroutine(CreatureController controller, float preMoveSpeed)
    {
        yield return new WaitForSeconds(5);
        controller._moveSpeed = preMoveSpeed;
    }
}
