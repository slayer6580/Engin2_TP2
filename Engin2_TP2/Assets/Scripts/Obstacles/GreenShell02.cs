using UnityEngine;
using static AudioManager;

public class GreenShell02 : MonoBehaviour
{
	[SerializeField] private float m_speed = 25f;
	[SerializeField] private float m_timeToDie = 10f;

	private Rigidbody m_rb;

	void Start()
	{
		m_rb = GetComponent<Rigidbody>();
		m_rb.AddForce(transform.forward * m_speed, ForceMode.Impulse);		
		Destroy(gameObject, m_timeToDie); // Destroys this game object after timeToDie seconds
	}

}