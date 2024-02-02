using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerTimer))]
public class PlayerCheckpoint : NetworkBehaviour
{
    private Vector3 m_spawnPoint;
    private Vector3 m_teleportPoint;
    private Rigidbody m_rb;

    private int m_checkpointReached = 0;

    private void Awake()
    {
        m_spawnPoint = transform.position;
        m_rb = GetComponent<Rigidbody>();
        m_teleportPoint = m_spawnPoint;
    }

    /// <summary> Pour la réapparition du joueur à son point de départ </summary>
    public void GoToTeleportPoint()
    {
        transform.SetPositionAndRotation(m_teleportPoint, Quaternion.identity);
        m_rb.velocity = Vector3.zero;
    }

    public void SetTeleportPoint()
    {
        m_teleportPoint = transform.position;
    }

    public int GetCheckpointReached()
    {
        return m_checkpointReached;
    }

    public void SetCheckpointReached(int number)
    {
        m_checkpointReached = number;
    }   
    
    public void SetTeleportPointToStart()
    {
        m_teleportPoint = m_spawnPoint;
    }
}
