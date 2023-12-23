using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimberScript : MonoBehaviour
{
    public PlayerMovementScript pScript;
    public EnemyBehaviorScript eScript;

    public bool isPlayer = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (isPlayer)
            {
                pScript.lockMovement = true;
            }
            else {
                eScript.stuckOnWall = true;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (isPlayer)
            {
                pScript.lockMovement = false;
            }
            else {
                eScript.stuckOnWall = false;
            }
        }
    }
}
