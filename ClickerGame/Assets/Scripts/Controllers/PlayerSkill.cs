using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : Skill
{
    public PlayerSkill(Dictionary<string, Data.Skill> skillDict) : base(skillDict)
    {
    }

    public void UseSkill(GameObject target)
    {
        string skillName = Managers.Skill.ChooseSkill();
        if (skillName == null)
            return;

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
