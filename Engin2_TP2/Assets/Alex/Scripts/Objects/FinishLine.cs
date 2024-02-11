using Mirror;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Voit t-on le renderer lors du lancement du jeu?")]
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

		if (character.enabled == false)
			return;

		if (character.GetCheckpointReached() != CheckpointManager.GetInstance().GetListLength())
            return; //Manque des checkpoints
        
        character.SetSpawnPointToStart();
        character.gameObject.GetComponent<PlayerTimer>().ResetTimer();
        character.ResetCheckpointReached();

        NetworkIdentity identity = character.gameObject.GetComponent<NetworkIdentity>();
        ScoreManager.GetInstance().CmdUpdateScore(ScoreManager.ETeam.runner, identity);
        AudioManager.GetInstance().CmdPlaySoundEffectsOneShotTarget(AudioManager.ESound.checkpoint, character.transform.position, identity);
	}

    /// <summary> Pour désactiver le renderer de l'objet </summary>
    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
