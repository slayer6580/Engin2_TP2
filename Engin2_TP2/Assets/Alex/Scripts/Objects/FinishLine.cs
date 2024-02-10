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
        character.GoToSpawnPoint();  
        character.gameObject.GetComponent<PlayerTimer>().ResetTimer();
        character.ResetCheckpointReached();

		ScoreManager.GetInstance().UpdateScoreCommand(ScoreManager.ETeam.runner, character.gameObject.GetComponent<NetworkIdentity>());
        AudioManager.GetInstance().PlaySoundEffectsOneShot_CMD(AudioManager.ESound.checkpoint, other.gameObject.transform.position);
	}

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
