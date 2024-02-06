using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerTimer))]
public class PlayerCheckpoint : NetworkBehaviour
{
    private Vector3 m_startPoint;
    private Vector3 m_spawnPoint;
    private Rigidbody m_rb;

    private int m_checkpointReached = 0;

    private void Awake()
    {
        m_startPoint = RunnerManager.GetInstance().FindAStartPoint();
        m_spawnPoint = m_startPoint;
        m_rb = GetComponent<Rigidbody>();       
    }

    private void Start()
    {
        GoToSpawnPoint();
    }

    /// <summary> Pour la réapparition du joueur à son point de départ </summary>
    public void GoToSpawnPoint()
    {
        transform.SetPositionAndRotation(m_spawnPoint, Quaternion.identity);
        m_rb.velocity = Vector3.zero;
    }

    /// <summary> Changer le point de réapparition </summary>
    public void SetSpawnPoint(Vector3 newPosition)
    {
        m_spawnPoint = newPosition;
    }

    /// <summary> Changer le point de réapparition pour le point de départ </summary>
    public void SetSpawnPointToStart()
    {
        m_spawnPoint = m_startPoint;
    }

    /// <summary> Pour savoir combien de checkpoint le joueur a franchi </summary>
    public int GetCheckpointReached()
    {
        return m_checkpointReached;
    }

    /// <summary> Pour augmenter le nombre de checkpoint que le joueur a franchi </summary>
    public void CheckpointReached()
    {
        m_checkpointReached++;
    }

    /// <summary> Pour remettre a 0 le nombre de checkpoint que le joueur a franchi </summary>
    public void ResetCheckpointReached()
    {
        m_checkpointReached = 0;
    }


}
