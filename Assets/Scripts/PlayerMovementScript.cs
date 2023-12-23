using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovementScript : MonoBehaviour
{
    // Objects
    private Rigidbody2D rb;

    private Camera mainCamera;
    public WorldClockScript worldClock;
    
    public GameObject aimPoint;
    public GameObject firePoint;

    private EntityScript entityScript;
    private CombatScript combatScript;
    private StoryProgressionDetectorScritp story;
    public PlayerSkillsScript skillsScript;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    private FacingDirection facingDir;
    
    private float nextAttack = 0;
    private float currTime = 0f;

    private float jumpAgain = 0f;
    private float jumpCooldown = 0.2f;

    private float skillIncCDR = 5f;
    private float cdrSkillsMovement = 0f;

    public bool lockMovement = false;

    public Light2D playerLight;
    
    public Skills skills;

    private float skillUpdateTime = 0f;
    
    public int currentStory = 0; 
    // Start is called before the first frame update
    void Start()
    {
        // Prevent unnecessary calls by storing variables
        rb = this.GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        entityScript = this.GetComponent<EntityScript>();
        
        combatScript = this.GetComponent<CombatScript>();

        story = GameObject.FindWithTag("StoryProgDetector").GetComponent<StoryProgressionDetectorScritp>();
        
        currentStory = story.storyProgression;
    }
    
    // Fixed update is called at same time always, independent of FPS.
    // Add movement force here to prevent higher FPS for faster movement. 
    private void FixedUpdate()
    {
        KeyboardMovement();

        GetFacingDirection();

        AimPointRotation();

        PlayerAttack();

        UpdateHealthBar();

        if (Time.realtimeSinceStartup > skillUpdateTime)
        {
            UpdateStatsBasedOnSkills();
            skillUpdateTime = Time.realtimeSinceStartup + 5;
        }
    }

    void UpdateHealthBar()
    {
        playerLight.intensity = entityScript.entityStats.healthPoints / entityScript.entityStats.maxHealthPoints;
    }
    
    void PlayerAttack()
    {
        
        currTime = Time.time;
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextAttack)
        {
            // Allow attacking based on story
            if (currentStory >= 5)
            {
                nextAttack = currTime + (1 / (entityScript.entityStats.attackSpeedMelee));
                combatScript.Atack(false);

                skills.melee += 1;            nextAttack = currTime + (1 / (entityScript.entityStats.attackSpeedMelee));
                combatScript.Atack(false);

                skills.melee += 1;
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1) && Time.time >= nextAttack)
        {
            if (currentStory >= 15)
            {
                nextAttack = currTime + (1 / (entityScript.entityStats.attackSpeedRanged));
                combatScript.Atack(true);

                skills.ranged += 1;
            }
        }
    }

    /**
     * Handles player death
     */
    public void PlayerDeath()
    {
        worldClock.ClockFinished(); // Reset clock
        worldClock.PlayRevert();
        
        if (currentStory < story.storyProgression)
        {
            story.PrintStoryText();
            currentStory = story.storyProgression; // Update story progression
        }
        
        playerLight.intensity = 1;
        
        skills.health += 1; // Increase skills
        UpdateStatsBasedOnSkills();
        entityScript.entityStats.healthPoints = entityScript.entityStats.maxHealthPoints;
    }

    void UpdateStatsBasedOnSkills()
    {
        skills = skillsScript.UpdateModifiers(skills);
        
        // Reset health points and update with skills
        entityScript.entityStats.maxHealthPoints = 1 + skills.healthModifier;

        
        entityScript.entityStats.meleeDamage = 1 + skills.meleeDmgModifier;
        entityScript.entityStats.attackSpeedMelee = 0.5f + skills.meleeAsModifier;
        
        entityScript.entityStats.rangeDamage = 1 + skills.rangedDmgModifier;
        entityScript.entityStats.attackSpeedRanged = 0.5f + skills.rangedAsModifier;
        
        entityScript.entityStats.jumpForce = 100 + skills.jumpingModifier;  // 5 is default value
        entityScript.entityStats.moveSpeed = 5 + skills.movementModifier * 0.8f; // 5 is default value
        entityScript.entityStats.maxMoveSpeed = 5 + skills.movementModifier; // 5 is default value
    }
    
    /**
     * Orients aimPoint and firePoint which are used for combat
     */
    void AimPointRotation()
    {
        // Get mouse position on screen based on camera and mouse position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Delete z values

        // Get direction of the mouse from this GameObject
        Vector3 direction = (mousePosition - transform.position).normalized;
        // Get angle between this GameObject and mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate aimPoint to the mouse
        aimPoint.transform.eulerAngles = new Vector3(0, 0, angle);
    }
    
    /**
     * Handles player movement like walking and jumping
     */
    void KeyboardMovement()
    {
        // Get input from horizontal axis A-D,Right-Left
        float xAxis = Input.GetAxis("Horizontal");
 
        var vel = Math.Abs(rb.velocity.x);
        // Check if player is not going too fast
        if (vel <= entityScript.entityStats.maxMoveSpeed)
        {
            // Prevent going into a wall which results in getting stuck on it
            if(!(lockMovement &&
                ((facingDir == FacingDirection.Right && xAxis > 0f) ||
                 (facingDir == FacingDirection.Left && xAxis < 0f))))
            {
                // Add force to the player
                rb.AddForce(xAxis * (entityScript.entityStats.moveSpeed) * Vector2.right, ForceMode2D.Impulse);
            }
        }

        // If player is moving faster than 1 increase his skills of movement  
        if (vel >= 1 && !lockMovement)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip);
            }

            if (Time.time > cdrSkillsMovement)
            {
                skills.movement += 1; // Increase skills
                cdrSkillsMovement = Time.time + skillIncCDR;
            }
        }
        
        // If player is on ground allow jumping
        if (Input.GetKey(KeyCode.Space))
        {
            if (entityScript.entityStats.grounded == true && Time.time > jumpAgain)
            {
                rb.AddForce(Vector2.up * (entityScript.entityStats.jumpForce), ForceMode2D.Impulse);
                jumpAgain = Time.time + jumpCooldown;
                skills.jumping += 1; // Increase skills
            }
        }
    }
    
    
    /**
     * Correctly faces player
     */
    void GetFacingDirection()
    {
        // Get where is player facing
        if (Input.GetKey(KeyCode.D))
        {
            facingDir = FacingDirection.Right;
            var rot = Quaternion.Euler(transform.localPosition);
            rot.y = 0;
            rot.x = 0;
            rot.z = 0;
            transform.localRotation = rot;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            facingDir = FacingDirection.Left;
            var rot = Quaternion.Euler(transform.localPosition);
            rot.y = 180;
            rot.x = 0;
            rot.z = 0;
            transform.localRotation = rot;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            facingDir = FacingDirection.Up;
        }
    }


}
