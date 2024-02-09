using Mirror;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CannonShooter : NetworkBehaviour
{
	[Header("Important")]
	[SerializeField] private ObstacleManager m_obstacleManager;
	[SerializeField] private GameObject cannonballPrefab; // Assign your cannonball prefab in the inspector
    [SerializeField] private Transform shootPoint; // Assign the point from where the cannonball will be shot

	[Header("Settings")]
	[SerializeField] private float shootingInterval = 2f; // x seconds between shots
    [SerializeField] private float cannonballSpeed = 1000f; // Speed of the cannonball
    [SerializeField] private float cannonballLifeSpan = 5f; // y seconds lifespan of the cannonball
    [SerializeField] private bool automaticFire = true;
    [SerializeField] private float cooldown = 2f;

    private float cooldownTimer = 0f; // Timer to track cooldown

	private float m_staminaCost;

	private void Start()
    {
        StartCoroutine(ShootCannonballRoutine());
    }

    private IEnumerator ShootCannonballRoutine()
    {
		yield return new WaitForSeconds(shootingInterval);

		if (automaticFire)
        {          
            ShootCannonball();
			StartCoroutine(ShootCannonballRoutine());
		}    
    }


    public void ShootCannonball()
    {
		m_staminaCost = m_obstacleManager.m_staminaCost;
        ShootCannonballCommand();
	}

	[Command(requiresAuthority = false)]
	public void ShootCannonballCommand()
    {
		GmStaminaManager.GetInstance().InstantCostCommand(m_staminaCost);

		GameObject cannonball = Instantiate(cannonballPrefab, shootPoint.position, shootPoint.rotation);

		// Apply a forward force to the cannonball to shoot it
		Rigidbody rb = cannonball.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(shootPoint.forward * cannonballSpeed);
		}
		NetworkServer.Spawn(cannonball, connectionToClient);

		// Destroy the cannonball after y seconds
		Destroy(cannonball, cannonballLifeSpan);
	}

}