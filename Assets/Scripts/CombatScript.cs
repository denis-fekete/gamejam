using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject meleeAttackPrefab;
    public GameObject rangedAttackPrefab;
    public GameObject[] firePoints;
    public EntityScript entityScript;
    
    public void Atack(bool isRanged)
    {

        foreach (var fp in firePoints)
        {
            GameObject clone;
            GameObject prefab = (isRanged) ? rangedAttackPrefab : meleeAttackPrefab;
            
            // Spawn attack
            clone = Instantiate(prefab, fp.transform.position, fp.transform.rotation);
        
            // Get script from spawned object
            AttackScript attackS = clone.GetComponent<AttackScript>();
            Rigidbody2D attackRb = clone.GetComponent<Rigidbody2D>();
        
            if (isRanged)
            {
                attackRb.AddForce(fp.transform.right * entityScript.entityStats.projectileSpeed, ForceMode2D.Impulse);
                attackS.timeToLive = entityScript.entityStats.rangedTimeToLive;                
                attackS.damage = entityScript.entityStats.rangeDamage;
            }
            else
            {
                attackRb.AddForce(fp.transform.right, ForceMode2D.Impulse);
                attackS.timeToLive = entityScript.entityStats.meleeTimeToLive;                
                attackS.damage = entityScript.entityStats.meleeDamage;
            }
        }
        
    }
}
