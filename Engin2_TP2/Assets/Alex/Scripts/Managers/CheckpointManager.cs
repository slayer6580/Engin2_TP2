using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Mettre tout les checkpoints en ordre dans cette liste")]
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
            Debug.LogError("Il y avait plus qu'une instance de StartPointManager dans la sc�ne, FIX IT!");
            Destroy(this);
        }
    }

    /// <summary> Pour v�rifier si un joueur peut prendre un checkpoint </summary>
    public bool ValidateCheckpoint(CheckPoint checkpoint, PlayerCheckpoint player)
    {
        if (player.GetCheckpointReached() == GetCheckpointNumber(checkpoint))
        {
            return true;
        }

        return false;
    }

    /// <summary> Pour avoir l'index du checkpoint dans la liste </summary>
    public int GetCheckpointNumber(CheckPoint checkpoint)
    {
        return m_checkpoints.IndexOf(checkpoint);
    }

    /// <summary> Pour savoir combien de checkpoint il y a dans le jeu </summary>
    public int GetListLength()
    {
        return m_checkpoints.Count;
    }

}
