using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroundColliderScript : MonoBehaviour
{

    public EntityScript entityScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            entityScript.entityStats.grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            entityScript.entityStats.grounded = false;
        }
    }
}
