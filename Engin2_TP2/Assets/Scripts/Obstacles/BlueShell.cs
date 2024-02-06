using UnityEngine;

public class BlueShell : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Transform target;
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
        
        if (playerStateMachine != null)
        {
            target = playerStateMachine.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
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