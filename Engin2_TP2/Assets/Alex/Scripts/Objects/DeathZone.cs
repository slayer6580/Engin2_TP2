using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private bool m_KeepRenderer;

    private void Awake()
    {
        DesactiveRenderer();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCheckpoint character = other.GetComponent<PlayerCheckpoint>();

        if (character == null)
            return;

        character.GoToTeleportPoint();
        character.gameObject.GetComponent<PlayerTimer>().ResetTimer();
        ScoreManager.GetInstance().CMD_ScoreGameMaster();
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
