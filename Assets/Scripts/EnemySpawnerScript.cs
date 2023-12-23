using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public GameObject spawnedEnemy;
        
    public bool isTriggered = false;
    public int id = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If hasn't been triggered, can be triggered
        if (isTriggered == false)
        {
            // If player collided with spawner, spawn enemy
            if (other.gameObject.CompareTag("Player"))
            {
                SpawnEnemy();
                isTriggered = true;
            }  
        }
    }

    void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemyPrefab, spawnPoint);
    }
}
