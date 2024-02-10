using UnityEngine;

public class BlueShell : MonoBehaviour
{
    [SerializeField] private float speed = 22f;

    private Transform target;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;
    [SerializeField] private float timeToDie = 25f;
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
            target = playerStateMachine.transform;
        }
        Destroy(gameObject, timeToDie);
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