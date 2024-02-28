using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerTimer))]
public class PlayerCheckpoint : NetworkBehaviour
{
    private Transform m_startPoint;
    private Transform m_spawnPoint;
    private Rigidbody m_rb;
    [SerializeField] private Transform m_body;
    [SerializeField] private Transform m_lookAt;

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
        transform.SetPositionAndRotation(m_spawnPoint.position, Quaternion.identity);
        m_rb.velocity = Vector3.zero;

        Vector3 targetEulerAngles = m_spawnPoint.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(0f, targetEulerAngles.y, 0f);
        m_body.rotation = targetRotation;
        m_lookAt.rotation = targetRotation;

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
