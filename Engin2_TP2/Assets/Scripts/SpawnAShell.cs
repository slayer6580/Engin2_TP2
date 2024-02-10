using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns a shell at gamemaster with different properties (red, blue, green)
public class SpawnAShell : MonoBehaviour
{
    [SerializeField] private GameObject greenShellPrefab;
    [SerializeField] private GameObject redShellPrefab;
    [SerializeField] private GameObject blueShellPrefab;
    [SerializeField] private float spawnInterval = 2f; // Time between spawns
    [SerializeField] private float spawnCooldown = 1f;

    private float timeSinceLastSpawn;

    private void Start()
    {
        timeSinceLastSpawn = spawnCooldown; 
    }

    void Update()
    {
        
        timeSinceLastSpawn += Time.deltaTime;

        
        if (Input.GetMouseButtonDown(1) && timeSinceLastSpawn >= spawnCooldown)
        {
            SpawnRandomShell();
            timeSinceLastSpawn = 0; // Reset the timer
        }
    }
    void SpawnRandomShell()
    {
        // Choose a random shell to spawn
        int shellType = Random.Range(0, 3); // Random number between 0 and 2
        GameObject shellToSpawn = null;

        switch (shellType)
        {
            case 0:
                shellToSpawn = greenShellPrefab;
                break;
            case 1:
                shellToSpawn = redShellPrefab;
                break;
            case 2:
                shellToSpawn = blueShellPrefab;
                break;
        }

        if (shellToSpawn != null)
        {
            // Spawn the shell at the camera's position and orientation
            Instantiate(shellToSpawn, Camera.main.transform.position, Camera.main.transform.rotation); // must be on gamemaster camera! also link the prefabs
        }
    }
}