using Mirror;
using UnityEngine;


public class PlayersManager : NetworkBehaviour
{
    [SerializeField] private Transform m_playerParent;

    private static PlayersManager s_instance = null;

    public static PlayersManager GetInstance()
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

    /// <summary> Place le joueur dans la hierachie dans la catégorie ----- Players -----  </summary>
    public void SetParent(GameObject _character)
    {
        _character.transform.SetParent(m_playerParent);
    }

}
