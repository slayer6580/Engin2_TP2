using Mirror;
using System.Collections.Generic;
using UnityEngine;


public class RunnerManager : NetworkBehaviour
{
    [Header("Mettre le transforn parent pour placer les runners dans la hierarchie")]
    [SerializeField] private Transform m_playerParent;
    [Header("Mettre tout les spawnpoints de mes runners")]
    [SerializeField] private List<Transform> m_spawnsPoints = new List<Transform>();

    private static RunnerManager s_instance = null;
    private int m_currentCheckpoint = 0;

    public static RunnerManager GetInstance()
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

        DeactivateSpawnPointText();
    }

    /// <summary> Place le joueur dans la hierarchie comme enfant d'un objet </summary>
    public void SetParent(GameObject _character)
    {
        _character.transform.SetParent(m_playerParent);
    }

    /// <summary> Trouve un point de départ pour un joueur </summary>
    public Vector3 FindAStartPoint()
    {
        if (m_spawnsPoints[m_currentCheckpoint] == null)
            m_currentCheckpoint = 0;

        Vector3 startPoint = m_spawnsPoints[m_currentCheckpoint].position;
        m_currentCheckpoint++;

        return startPoint;
        
    }

    /// <summary> Désactive le texte des spawnPoints </summary>
    private void DeactivateSpawnPointText()
    {
        foreach (Transform spawn in m_spawnsPoints)
        {
            spawn.GetChild(0).gameObject.SetActive(false);
        }
    }

}
