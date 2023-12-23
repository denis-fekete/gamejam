using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldClockScript : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    public string clockTextString = "Remaining time: ";
    
    public bool clockActive = false;
    private float clockStart = 0f;
    public float clockEnd = 60f;
    public float notifyPlayer = 10f;

    public GameObject player;
    public Transform playerSpawn;


    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip revert;
    public bool soundPlaying = false;
    public EnemySpawnerScript[] spawners;

    public bool gamePaused = true;
    public GameObject menuCanvas;

    private float timePauseDelta = 0;
    private float timeAfterPause = 0f;
    private float timeBeforePause = 0f;


    private void Start()
    {
        PauseGame();
    }

    // Use Update for keyboard input because its not affected by timeScale
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (gamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (gamePaused && Input.GetKeyDown(KeyCode.Q))
        {
            ExitGame();
        }
    }

    void FixedUpdate()
    {
        if (!gamePaused)
        {
            if (clockActive)
            {
                if (Time.realtimeSinceStartup >= clockStart + clockEnd + timePauseDelta)
                {
                    ClockFinished();
                } 
                else if (Time.realtimeSinceStartup >= clockStart + clockEnd - notifyPlayer && !soundPlaying)
                {
                    StartClockTicking();
                    soundPlaying = true;
                }

                UpdateClock();
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    StartClock();
                }
            }
        }
        
    }

    public void UnpauseGame()
    {
        gamePaused = false;
        menuCanvas.SetActive(false);

        timeAfterPause = Time.realtimeSinceStartup;

        timePauseDelta = timeAfterPause - timeBeforePause;
        
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        gamePaused = true;
        menuCanvas.SetActive(true);
        
        timeBeforePause = Time.realtimeSinceStartup;
        Time.timeScale = 0;
    }
    

    public void ExitGame()
    {
        Application.Quit();
    }
    
    private void UpdateClock()
    {
        clockText.text = ((clockTextString + (int)(clockStart + clockEnd + timePauseDelta - Time.realtimeSinceStartup))).ToString();
    }

    void StartClockTicking()
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayRevert()
    {
        audioSource.PlayOneShot(revert);
    }
    
    public void ClockFinished()
    {
        // TODO: Play some animation

        // Reset all enemy spawners
        foreach (var spawner in spawners)
        {
            Destroy(spawner.spawnedEnemy);
            spawner.isTriggered = false;
        }
        
        player.transform.position = playerSpawn.position;
        ResetClock();
        StartClock();
    }
    
    void StartClock()
    {
        clockActive = true;
        clockStart = Time.realtimeSinceStartup;
        soundPlaying = false;
        timePauseDelta = 0;
    }
    
    void ResetClock()
    {
        clockActive = false;
    }
}
