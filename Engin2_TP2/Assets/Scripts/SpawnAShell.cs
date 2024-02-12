using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

// Spawns a shell at gamemaster with different properties (red, blue, green)
public class SpawnAShell : NetworkBehaviour
{
    [SerializeField] private GameObject greenShellPrefab;
    [SerializeField] private GameObject redShellPrefab;
    [SerializeField] private GameObject blueShellPrefab;
    [SerializeField] private float spawnInterval = 2f; // Time between spawns
    [SerializeField] private float spawnCooldown = 1f;
    [SerializeField] private float m_staminaCost;

    private float timeSinceLastSpawn;
	private Ray ray;
	private RaycastHit hit;
	private void Start()
    {
        timeSinceLastSpawn = spawnCooldown; 
    }

    void Update()
    {
        
        timeSinceLastSpawn += Time.deltaTime;
     
        if (Input.GetMouseButtonDown(1) && timeSinceLastSpawn >= spawnCooldown)
        {
			if (GmStaminaManager.GetInstance().CanUseStamina(m_staminaCost))
            {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, Mathf.Infinity))
				{
					Vector3 target = hit.point;

					GmStaminaManager.GetInstance().InstantCostCommand(m_staminaCost);
					CmdSpawnRandomShell(target, Camera.main.transform.position);
					timeSinceLastSpawn = 0; // Reset the timer
				}
			}
				
        }
    }

	[Command(requiresAuthority = false)]
	void CmdSpawnRandomShell(Vector3 target, Vector3 spawnPosition)
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
			GameObject shell = Instantiate(shellToSpawn, spawnPosition,Quaternion.identity ); // must be on gamemaster camera! also link the prefabs
			shell.transform.LookAt(target);

			NetworkServer.Spawn(shell, connectionToClient);
		}
	}
}