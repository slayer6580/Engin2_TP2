using UnityEngine;
using static AudioManager;

public class GreenShell02 : MonoBehaviour
{
	[SerializeField] private float speed = 25f;
	[SerializeField] private float timeToDie = 10f;

	private Rigidbody rb;
	private PlayerStateMachine playerStateMachine;
	private void Awake()
	{
		AudioManager.GetInstance().CmdPlaySoundEffectsOneShotAll(ESound.throwShell, gameObject.transform.position);
		FindTarget();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		rb.AddForce(transform.forward * speed, ForceMode.Impulse);
		
		Destroy(gameObject, timeToDie); // Destroys this game object after timeToDie seconds
	}

	void FindTarget()
	{
		PlayerStateMachine playerStateMachine = FindObjectOfType<PlayerStateMachine>();
	}
	void OnCollisionEnter(Collision collision)
	{
		//Destroy(gameObject);
	}

}