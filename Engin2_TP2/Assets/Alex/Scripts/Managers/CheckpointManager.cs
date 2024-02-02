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

    public bool ValidateCheckpoint(CheckPoint checkpoint, PlayerCheckpoint player)
    {
        int checkpointNumber = m_checkpoints.IndexOf(checkpoint) + 1;

        if (player.GetCheckpointReached() + 1 == checkpointNumber)
        {
            return true;
        }

        return false;
    }

    public int GetCheckpointNumber(CheckPoint checkpoint)
    {
        return m_checkpoints.IndexOf(checkpoint) + 1;
    }

    public int GetListLength()
    {
        return m_checkpoints.Count;
    }

    private void OnValidate()
    {
        for (int i = 0; i < m_checkpoints.Count; i++)
        {
            m_checkpoints[i].gameObject.name = "Checkpoint" + (i + 1).ToString();
        }
    }
}
