using UnityEngine;

public class PlayerSkill : Skill
{
    public void UseSkill(GameObject target)
    {
        string skillName = Managers.Skill.ChooseSkill();
        if (skillName == null)
        {
            //Debug.Log("You don't have Skill");
            return;
        }

        Debug.Log(skillName);

        switch (skillName)
        {
            case "Knockback":
                Knockback(target);
                break;
            case "Slow":
                Slow(target);
                break;
        }
    }
}
