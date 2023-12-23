using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum Tags {Player, Enemy} 

[Serializable]
public struct Stats
{
    public Tags tag;
    public float healthPoints;
    public float maxHealthPoints;
    
    //
        
    public float attackSpeedMelee;
    public float meleeDamage;
    public float meleeTimeToLive;
    
    public float attackSpeedRanged;
    public float rangeDamage;
    public float projectileSpeed;
    public float rangedTimeToLive;
    
    //
    public float moveSpeed;
    public float maxMoveSpeed;
    
    public float weight;
    public float jumpForce;
    public bool grounded;
}

public enum FacingDirection
{
    Right,
    Left,
    Up
};

public class EntityScript : MonoBehaviour
{
    public PlayerMovementScript playerScript;
    public Stats entityStats;
    public bool diedInCombat = false;
    public int threatLevel = 0;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (entityStats.tag == Tags.Player)
        {
            
            if (other.collider.CompareTag("EnemyAttack"))
            {
                var attackProperties = other.gameObject.GetComponent<AttackScript>();
            
                IsAttacked(attackProperties.damage);
                
                if (attackProperties.destroyOverTime)
                {
                    Destroy(other.gameObject); 
                }
            }
        }
        else if (entityStats.tag == Tags.Enemy)
        {
            if (other.collider.CompareTag("PlayerAttack"))
            {
                var attackProperties = other.gameObject.GetComponent<AttackScript>();
            
                IsAttacked(attackProperties.damage);

                if (attackProperties.destroyOverTime)
                {
                    Destroy(other.gameObject); 
                }
            }
        }

    }

    private void IsAttacked(float damage)
    {
        entityStats.healthPoints -= damage;
        
        if (entityStats.tag == Tags.Player)
        {
            if (entityStats.healthPoints <= 0)
            {
                playerScript.PlayerDeath();
            }
            
        }
        else if (entityStats.tag == Tags.Enemy)
        {
            if (entityStats.healthPoints <= 0)
            {
                diedInCombat = true;
                Destroy(this.gameObject);
            }
        }
    }

    
    private void OnDestroy()
    {
        // If entity died and its an enemy
        if (diedInCombat && entityStats.tag == Tags.Enemy)
        {
            // Find StoryProgDetector
            StoryProgressionDetectorScritp story = GameObject.FindWithTag("StoryProgDetector").GetComponent<StoryProgressionDetectorScritp>();
            // Call update story
            story.UpdateStory(threatLevel);
        }
    }
}
