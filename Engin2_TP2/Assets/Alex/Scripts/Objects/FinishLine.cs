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

		ScoreManager.GetInstance().CmdUpdateScore(ScoreManager.ETeam.runner, character.gameObject.GetComponent<NetworkIdentity>());
        AudioManager.GetInstance().CmdPlaySoundEffectsOneShot(AudioManager.ESound.checkpoint, other.gameObject.transform.position);
	}

    /// <summary> Pour désactiver le renderer de l'objet </summary>
    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
