using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlBasic : MonoBehaviour
{
    [Header("Vitesse de mouvement")]
    [SerializeField] private float m_movementSpeed;
    [Header("Vitesse de rotation de la caméra")]
    [SerializeField] private float m_rotationSpeed;
    private Rigidbody m_rigidbody;
    private bool m_isMoving = false;
    private Vector3 m_velocity;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    [ClientCallback] // makes sure that it's not run on the server
    void Update()
    {
        MovementInputs();
        RotationInputs();
    }

    [ClientCallback] // makes sure that it's not run on the server
    private void FixedUpdate()
    {
        PlayerMovement_FU();
    }

    private void RotationInputs()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, m_rotationSpeed * mouseX);
    }

    private void MovementInputs()
    {
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");

        m_isMoving = xMov != 0 || yMov != 0 ? true : false;

        Vector3 horizontalMov = transform.right * xMov;
        Vector3 verticalMov = transform.forward * yMov;

        m_velocity = (horizontalMov + verticalMov).normalized * m_movementSpeed;

    }

    private void PlayerMovement_FU()
    {
        if (m_isMoving)
            m_rigidbody.AddForce(m_velocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
