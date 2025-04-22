using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private Data.Skill _skillKnockback;
    private Data.Skill _skillSlow;

    public Skill(Dictionary<string, Data.Skill> skillDict)
    {
        _skillKnockback = skillDict["Knockback"];
        _skillSlow = skillDict["Slow"];
    }

    public virtual void Knockback(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();

        float endPosX = target.transform.position.x + _skillKnockback.skillValue;
        float moveSpeed = 5f;
        controller.Move(endPosX, moveSpeed);
    }

    public virtual void Slow(GameObject target)
    {
        CreatureController controller = target.GetComponent<CreatureController>();

        float preMoveSpeed = controller._moveSpeed;
        controller._moveSpeed *= _skillSlow.skillValue;
        StartCoroutine(SlowCoroutine(controller, preMoveSpeed));
    }

    protected IEnumerator SlowCoroutine(CreatureController controller, float preMoveSpeed)
    {
        yield return new WaitForSeconds(5);
        controller._moveSpeed = preMoveSpeed;
    }
}
