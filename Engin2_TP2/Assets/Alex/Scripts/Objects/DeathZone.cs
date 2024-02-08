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
        ScoreManager.GetInstance().UpdateScore(ScoreManager.ETeam.gameMaster);
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
