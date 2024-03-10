using UnityEngine;

public class GreenShell : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;
    private void Awake()
    {
        FindTarget();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find the player by the PlayerStateMachine component
        PlayerStateMachine playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        if (playerStateMachine != null)
        {
            Vector3 targetDirection = (playerStateMachine.transform.position - transform.position).normalized;
            rb.velocity = targetDirection * speed;
        }
    }
   
    void FindTarget()
    {
        PlayerStateMachine playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}