using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterSpawnPoint : MonoBehaviour
{
    private Vector3 m_spawnPoint;
    private Rigidbody m_rb;

    private void Awake()
    {
        m_spawnPoint = transform.position;
        m_rb = GetComponent<Rigidbody>();
    }

    public void GoToSpawnPoint()
    {
        transform.position = m_spawnPoint;
        m_rb.velocity = Vector3.zero;
    }
}
