using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float m_bonusToAdd;
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

        CheckpointManager instance = CheckpointManager.GetInstance();

        if (!instance.ValidateCheckpoint(this, character))
        {
            Debug.LogError("Manque des checkpoints!");
            return;
        }

        character.SetCheckpointReached(instance.GetCheckpointNumber(this));
        ScoreManager.GetInstance().CMD_ScoreRunner();
        character.GetComponent<PlayerTimer>().AddBonusToTimer(m_bonusToAdd);
        character.SetTeleportPoint();
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
        GetComponent<MeshRenderer>().enabled = false;
    }
}
