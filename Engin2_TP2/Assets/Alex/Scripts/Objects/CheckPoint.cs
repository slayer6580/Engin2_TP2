using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Le nombre de temps bonus à donner au runner")]
    [SerializeField] private float m_bonusToAdd;
    [Header("Voit t-on le renderer lors du lancement du jeu?")]
    [SerializeField] private bool m_KeepRenderer;
    [Header("Mettre les spawns du checkpoint ici")]
    [SerializeField] private List<Transform> m_checkpointSpawns;

    private void Awake()
    {
        DesactiveRenderer();
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerCheckpoint character = other.GetComponent<PlayerCheckpoint>();

        if (character == null)
            return;

        if (m_checkpointSpawns.Count == 0)
        {
            Debug.LogError("Pas de spawn dans la liste");
            return;
        }

        CheckpointManager instance = CheckpointManager.GetInstance();

        if (!instance.ValidateCheckpoint(this, character))
        {
            Debug.LogError("Manque des checkpoints!");
            return;
        }

        character.CheckpointReached();
        ScoreManager.GetInstance().ScoreRunner();
        character.GetComponent<PlayerTimer>().AddBonusToTimer(m_bonusToAdd);
        character.SetSpawnPoint(GetRandomSpawnPosition());
    }

    private void DesactiveRenderer()
    {
        if (!m_KeepRenderer)
            GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary> Retourne la position d'un spawn au hazard </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        int randomIndex = Random.Range(0, m_checkpointSpawns.Count);
        return m_checkpointSpawns[randomIndex].position;
    }

}
