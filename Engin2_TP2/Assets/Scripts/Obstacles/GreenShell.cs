using UnityEngine;

public class GreenShell : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float timeToDie = 10f;

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

        /* 
         * NOTE: S'il y a plus qu'un player, il faudrait une fonction pour targetter le player le plus proche de la caméra
         */
        PlayerStateMachine playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        if (playerStateMachine != null)
        {
            Vector3 targetDirection = (playerStateMachine.transform.position - transform.position).normalized;
            rb.velocity = targetDirection * speed;
        }
        Destroy(gameObject, timeToDie); // Destroys this game object after timeToDie seconds
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