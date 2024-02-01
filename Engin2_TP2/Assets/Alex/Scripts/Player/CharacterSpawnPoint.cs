using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerTimer))]
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
        transform.SetPositionAndRotation(m_spawnPoint, Quaternion.identity);
        m_rb.velocity = Vector3.zero;
        GetComponent<PlayerTimer>().ResetTimer();
    }


    private void SetSpawnPoint()
    {
        m_spawnPoint = transform.position;
    }

}
