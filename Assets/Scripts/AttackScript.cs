using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float damage;
    public float timeToLive;
    public bool destroyOverTime = true;
    
    private float timeCreated;
    

    
    private void Start()
    {
        timeCreated = Time.time;
    }

    void FixedUpdate()
    {
        // Check if object/attack should be destroyed
        if (destroyOverTime)
        {
            if (Time.time - timeCreated >= timeToLive)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
