using Mirror;
using UnityEngine;


public class PlayersManager : NetworkBehaviour
{
    [SerializeField] private Transform[] m_startPoint;
    [SerializeField] private Transform m_playerParent;

    private static PlayersManager s_instance = null;
    public readonly SyncDictionary<string, GameObject> m_players = new SyncDictionary<string, GameObject>();

    [Header("Read Only")]
    [SyncVar][SerializeField] private int m_playersCount = 0;

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

    [Command(requiresAuthority = false)]
    public void Cmd_AddPlayer(GameObject player)
    {
        string playerId = "Player" + player.GetComponent<NetworkIdentity>().netId.ToString();
        player.GetComponent<PlayerSetup>().m_name = playerId;
        m_players.Add(playerId, player);
        m_playersCount = m_players.Count;
        Debug.Log("Added Player: " + playerId);
        SetNamesOnConnect(player);
    }

    public void SetNamesOnConnect(GameObject player)
    {
        player.GetComponent<PlayerSetup>().Rpc_SetNameOnConnect();
    }

    [Command(requiresAuthority = false)]
    public void Cmd_RemovePlayer(GameObject player)
    {
        m_players.Remove(player.name);
        m_playersCount = m_players.Count;
    }

    public void SetParent(GameObject _character)
    {
        _character.transform.SetParent(m_playerParent);
    }





}
