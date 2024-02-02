using UnityEngine;

public class FinishLine : MonoBehaviour
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

        if (character.GetCheckpointReached() != CheckpointManager.GetInstance().GetListLength())
        {
            Debug.LogError("Manque des checkpoints!");
            return;
        }

        character.SetTeleportPointToStart();
        character.GoToTeleportPoint();  
        character.gameObject.GetComponent<PlayerTimer>().ResetTimer();
        character.SetCheckpointReached(0);

        ScoreManager.GetInstance().CMD_ScoreRunner();
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
