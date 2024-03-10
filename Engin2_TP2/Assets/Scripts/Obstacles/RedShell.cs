using UnityEngine;

public class RedShell : MonoBehaviour
{

    // Note: When called, spawn it at gamemaster camera. Add cooldown on red shell if needed
    [SerializeField] private float speed = 5f;
    [SerializeField] private float homingDuration = 5f; // Duration the cannonball will follow the player
     

    private Transform target;
    private float homingTimer;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        FindTarget();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        homingTimer = homingDuration;

      
        if (playerStateMachine != null)
        {
            target = playerStateMachine.transform;
        }
    }

    void Update()
    {
        if (target != null && homingTimer > 0)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
            homingTimer -= Time.deltaTime;
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