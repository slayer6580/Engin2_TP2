using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

// Spawns a shell at gamemaster with different properties (red, blue, green)
public class SpawnAShell : NetworkBehaviour
{
    [SerializeField] private GameObject m_greenShellPrefab;
    [SerializeField] private GameObject m_redShellPrefab;
    [SerializeField] private GameObject m_blueShellPrefab;
    [SerializeField] private float m_spawnInterval = 2f; // Time between spawns
    [SerializeField] private float m_spawnCooldown = 1f;
    [SerializeField] private float m_staminaCost;

    private float m_timeSinceLastSpawn;
	private Ray m_ray;
	private RaycastHit m_hit;
	private void Start()
    {
        m_timeSinceLastSpawn = m_spawnCooldown; 
    }

    void Update()
    {
        
        m_timeSinceLastSpawn += Time.deltaTime;
     
        if (Input.GetMouseButtonDown(1) && m_timeSinceLastSpawn >= m_spawnCooldown)
        {
			if (GmStaminaManager.GetInstance().CanUseStamina(m_staminaCost))
            {
				m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(m_ray, out m_hit, Mathf.Infinity))
				{
					Vector3 target = m_hit.point;

					GmStaminaManager.GetInstance().InstantCostCommand(m_staminaCost);
					CmdSpawnRandomShell(target, Camera.main.transform.position);
					m_timeSinceLastSpawn = 0; // Reset the timer
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
                shellToSpawn = m_greenShellPrefab;
                break;
            case 1:
                shellToSpawn = m_redShellPrefab;
                break;
            case 2:
                shellToSpawn = m_blueShellPrefab;
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