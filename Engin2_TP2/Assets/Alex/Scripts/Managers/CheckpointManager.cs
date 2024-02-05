using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<CheckPoint> m_checkpoints = new List<CheckPoint>();

    private static CheckpointManager s_instance = null;

    public static CheckpointManager GetInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Debug.LogError("Il y avait plus qu'une instance de StartPointManager dans la scène, FIX IT!");
            Destroy(this);
        }
    }

    /// <summary> Pour vérifier si le joueur peut prendre ce checkpoint </summary>
    public bool ValidateCheckpoint(CheckPoint checkpoint, PlayerCheckpoint player)
    {
        if (player.GetCheckpointReached() == GetCheckpointNumber(checkpoint))
        {
            return true;
        }

        return false;
    }

    /// <summary> Pour avoir l'index du checkpoint dans la liste excluant le 0 </summary>
    public int GetCheckpointNumber(CheckPoint checkpoint)
    {
        return m_checkpoints.IndexOf(checkpoint);
    }

    /// <summary> Pour savoir combien de checkpoint il y a dans le jeu </summary>
    public int GetListLength()
    {
        return m_checkpoints.Count;
    }

    private void OnValidate()
    {
        for (int i = 0; i < m_checkpoints.Count; i++)
        {
            m_checkpoints[i].gameObject.name = "Checkpoint_" + (i + 1).ToString();
        }
    }
}
