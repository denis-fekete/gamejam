using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Skills
{
    public int melee;
    public int ranged;
    public int movement;
    public int jumping;
    public int health;

    public float meleeDmgModifier;
    public float meleeAsModifier;
    public float rangedDmgModifier;
    public float rangedAsModifier;
    public float movementModifier;
    public float jumpingModifier;
    public float healthModifier;
}

public class PlayerSkillsScript : MonoBehaviour
{
    public Skills UpdateModifiers(Skills skills)
    {
        skills = UpdateMelee(skills);
        skills = UpdateRanged(skills);
        skills = UpdateMovement(skills);
        skills = UpdateHealth(skills);
        skills = UpdateJumping(skills);

        return skills;
    }

    Skills UpdateMelee(Skills skills)
    {
        skills.meleeDmgModifier = skills.melee / 3.5f;
        skills.meleeDmgModifier = Mathf.Log10(Mathf.Max(1, skills.melee));
            
        return skills;
    }

    Skills UpdateRanged(Skills skills)
    {
        skills.rangedDmgModifier = skills.ranged / 10f;
        skills.rangedAsModifier = Mathf.Log10(Mathf.Max(1, skills.ranged));
        
        return skills;
    }

    Skills UpdateHealth(Skills skills)
    {
        // After 20th death it could be frustrating for player, so
        // get more health
        if (skills.health > 20f)
        {
            skills.healthModifier = 20f * skills.health;
        }
        else
        {
            skills.healthModifier = skills.health;
        }
        return skills;
    }
    
    Skills UpdateMovement(Skills skills)
    {
        // After 20th death it could be frustrating for player, so
        // get more health
        if (skills.movement > 40)
        {
            skills.movementModifier = 8 + Mathf.Log10(Mathf.Max(1, skills.movement));
        }
        else
        {
            skills.movementModifier = skills.movement / 5f;
        }
        return skills;
    }
    
    Skills UpdateJumping(Skills skills)
    {
        if (skills.jumping >= 220)
        {
            skills.jumpingModifier = 50 + Mathf.Log10(Mathf.Max(1, skills.jumping));
        }
        else
        {
            // Magic constant that works good
            skills.jumpingModifier = skills.jumping / 5f;
        }
        
        return skills;
    }
}
