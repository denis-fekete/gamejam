using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    // Objects
    private Rigidbody2D rb;
    
    private GameObject player;
    
    public GameObject aimPoint;
    public GameObject firePoint;

    private Vector2 playerDirection;

    public Stats eStats;
    public float attackDistance = 0;    

    private EntityScript entityScript;
    public CombatScript combatScript;
    
    private FacingDirection facingDir;
    
    private float nextAttack = 0;
    private float currTime = 0f;

    public bool isRangedType;

    public bool stuckOnWall = false;
    
    public float jumpCooldown = 1;
    private float jumpAgain = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Prevent unnecessary calls by storing variables
        rb = this.GetComponent<Rigidbody2D>();

        entityScript = this.GetComponent<EntityScript>();
        eStats = entityScript.entityStats;
        
        // Find player in scene
        player = GameObject.Find("Player");
    }
    
    // Fixed update is called at same time always, independent of FPS.
    // Add movement force here to prevent higher FPS for faster movement. 
    private void FixedUpdate()
    {
        AimPointRotation();
        GetFacingDirection();
        
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < attackDistance)
        {
            EnemyAttack();
        }
        else
        {
            EnemyMovement();
        }
    }

    void EnemyAttack()
    {
        currTime = Time.time;
        if (Time.time >= nextAttack)
        {
            combatScript.Atack(isRangedType);
            
            var attackSpeed = (isRangedType) ? eStats.attackSpeedRanged : eStats.attackSpeedMelee; 
            nextAttack = currTime + (1 / attackSpeed);
        }
    }
    
    void EnemyMovement()
    {
        float dir = (playerDirection.x > 0f) ? 1f : -1f; 
        rb.AddForce(eStats.moveSpeed * dir * Vector2.right);
        
        if ((playerDirection.y > 0.8f || stuckOnWall) && Time.time > jumpAgain)
        {
            UpdateEStats();
            // If player is on ground allow jumping
            if (eStats.grounded == true)
            {
                rb.AddForce(Vector2.up * eStats.jumpForce);
                jumpAgain = Time.time + jumpCooldown;
            }
        }
    }

    void UpdateEStats()
    {
        eStats = entityScript.entityStats;
    }
    
    void AimPointRotation()
    {
        // Get direction of the player from this gameobject
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // Get angle between this gameobject and mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate aimPoint to the mouse
        aimPoint.transform.eulerAngles = new Vector3(0, 0, angle);

        playerDirection = direction;
    }
    
    void GetFacingDirection()
    {
        // Get where is player facing
        if (playerDirection.x > 0f )
        {
            facingDir = FacingDirection.Right;
            var rot = Quaternion.Euler(transform.localPosition);
            rot.y = 0;
            transform.localRotation = rot;
        }
        else if (playerDirection.x < 0f)
        {
            facingDir = FacingDirection.Left;
            var rot = Quaternion.Euler(transform.localPosition);
            rot.y = 180;
            transform.localRotation = rot;
        }
    }
}
