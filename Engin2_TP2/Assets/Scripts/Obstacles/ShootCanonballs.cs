using Mirror;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CannonShooter : NetworkBehaviour
{
    [SerializeField] private GameObject cannonballPrefab; // Assign your cannonball prefab in the inspector
    [SerializeField] private Transform shootPoint; // Assign the point from where the cannonball will be shot
    [SerializeField] private float shootingInterval = 2f; // x seconds between shots
    [SerializeField] private float cannonballSpeed = 1000f; // Speed of the cannonball
    [SerializeField] private float cannonballLifeSpan = 5f; // y seconds lifespan of the cannonball

    [SerializeField] private bool automaticFire = true;
    [SerializeField] private float cooldown = 2f;
    private float cooldownTimer = 0f; // Timer to track cooldown

    private void Start()
    {
        StartCoroutine(ShootCannonballRoutine());
    }

    private IEnumerator ShootCannonballRoutine()
    {

        if (automaticFire)
        {
            yield return new WaitForSeconds(shootingInterval);
            ShootCannonball();
			StartCoroutine(ShootCannonballRoutine());
		}
        
    }


    public void ShootCannonball()
    {
        ShootCannonballCommand();
	}

	[Command(requiresAuthority = false)]
	public void ShootCannonballCommand()
    {
        ClientShootCannonball();
	}

    //TODO � changer pour un spawn par network???
	[ClientRpc]
    public void ClientShootCannonball()
    {

        GameObject cannonball = Instantiate(cannonballPrefab, shootPoint.position, shootPoint.rotation);
        // Apply a forward force to the cannonball to shoot it
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * cannonballSpeed);
        }
        // Destroy the cannonball after y seconds
        Destroy(cannonball, cannonballLifeSpan);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the cannon itself was clicked
                if (hit.transform == transform)
                {
                    ShootCannonball();
                    cooldownTimer = cooldown; // Reset the cooldown timer
                }
            }
        }
    }
}