using UnityEngine;

public class RedShell : MonoBehaviour
{

    // Note: When called, spawn it at gamemaster camera. Add cooldown on red shell if needed
    [SerializeField] private float speed = 20f;
    [SerializeField] private float homingDuration = 5f; // Duration the cannonball will follow the player
    [SerializeField] private float timeToDie = 15f;

    private Transform target;
    private float homingTimer=5.0f;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        //On doit pouvoir trouver le joueur le plus proche avant de l'intégré
        FindTarget();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        homingTimer = homingDuration;

        PlayerStateMachine playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        if (playerStateMachine != null)
        {
            target = playerStateMachine.transform;
        }
        Destroy(gameObject, timeToDie); // Destroys this shell after timeToDie seconds

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
       // Destroy(gameObject,0.1f);
    }
}