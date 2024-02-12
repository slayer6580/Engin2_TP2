using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerTimer))]
public class PlayerCheckpoint : NetworkBehaviour
{
    private Transform m_startPoint;
    private Transform m_spawnPoint;
    private Rigidbody m_rb;
    [SerializeField] private Transform m_lookAt;
    [SerializeField] private Transform m_body;

    private int m_checkpointReached = 0;

    private void Awake()
    {
        Transform startPoint = RunnerManager.GetInstance().FindAStartPoint();
        m_startPoint = startPoint;
        m_lookAt.rotation = startPoint.rotation;
        m_body.rotation = startPoint.rotation;
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
        transform.SetPositionAndRotation(m_spawnPoint.position, Quaternion.identity);
        m_rb.velocity = Vector3.zero;
    }

    /// <summary> Changer le point de réapparition </summary>
    public void SetSpawnPoint(Transform newPosition)
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
