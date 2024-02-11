using Mirror;
using UnityEngine;

public class DeathZone : MonoBehaviour
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

		character.GoToSpawnPoint();
        character.gameObject.GetComponent<PlayerTimer>().ResetTimer();
        character.gameObject.GetComponent<StaminaPlayer>().ResetStamina();
        character.gameObject.GetComponent<StaminaPlayer>().SetStaminaUI();
        NetworkIdentity identity = character.gameObject.GetComponent<NetworkIdentity>();
        ScoreManager.GetInstance().CmdUpdateScore(ScoreManager.ETeam.gameMaster, identity);
        AudioManager.GetInstance().CmdPlaySoundEffectsOneShotTarget(AudioManager.ESound.checkpoint, character.transform.position, identity);
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
