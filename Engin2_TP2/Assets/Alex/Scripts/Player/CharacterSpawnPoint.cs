using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterSpawnPoint : NetworkBehaviour
{
    private Vector3 m_spawnPoint;
    private Rigidbody m_rb;

    private void Awake()
    {
        SetSpawnPoint();
        m_rb = GetComponent<Rigidbody>();
    }

    /// <summary> Pour la réapparition du joueur à son point de départ </summary>
    public void GoToSpawnPoint()
    {
        transform.position = m_spawnPoint;
        m_rb.velocity = Vector3.zero;
    }

    [Client] 
    private void SetSpawnPoint()
    {
        m_spawnPoint = transform.position;
    }

}
