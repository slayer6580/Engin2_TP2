using Mirror;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    public enum ETeam
    {
        gameMaster,
        runner
    }

    [SerializeField] private TextMeshProUGUI m_gamemasterScoreText;
    [SerializeField] private TextMeshProUGUI m_runnerScoreText;

    [SyncVar] private int m_gamemasterScore = 0;
    [SyncVar] private int m_runnerScore = 0;

    private static GameManager s_instance = null;


    public static GameManager GetInstance()
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
    public void CMD_ScoreGameMaster()
    {
        RPC_UpdateScore(ETeam.gameMaster);
    }

    [Command(requiresAuthority = false)]
    public void CMD_ScoreRunner()
    { 
        RPC_UpdateScore(ETeam.runner);
    }

    [ClientRpc]
    private void RPC_UpdateScore(ETeam team)
    {
        if (team == ETeam.gameMaster)
            m_gamemasterScore++;
        else
            m_runnerScore++;

        m_gamemasterScoreText.text = m_gamemasterScore.ToString();
        m_runnerScoreText.text = m_runnerScore.ToString();
    }

}
