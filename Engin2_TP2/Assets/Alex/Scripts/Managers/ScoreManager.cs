using Mirror;
using TMPro;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{

    public enum ETeam
    {
        gameMaster,
        runner
    }

    [Header("TextMeshProGUI du Score Canvas")]
    [SerializeField] private TextMeshProUGUI m_gamemasterScoreText;
    [SerializeField] private TextMeshProUGUI m_runnerScoreText;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private TextMeshProUGUI m_winnerText;
    [SerializeField] private int m_maxScore;

    private int m_gamemasterScore = 0;
	private int m_runnerScore = 0;

    private static ScoreManager s_instance = null;
    private bool m_gameIsOver = false;
    


    public static ScoreManager GetInstance()
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
    public void UpdateScoreCommand(ETeam team, NetworkIdentity player)
    {
        UpdateScoreRpc(team);
        CheckForWinners_TRPC(player.connectionToClient);
	}


    /// <summary> Pour augmenter le score d'une équipe et le gerer dans le UI </summary>

    [ClientRpc]
    private void UpdateScoreRpc(ETeam team)
    {
        if (team == ETeam.gameMaster)
            m_gamemasterScore++;
        else
            m_runnerScore++;

        m_gamemasterScoreText.text = m_gamemasterScore.ToString();
        m_runnerScoreText.text = m_runnerScore.ToString();
     
    }

    [TargetRpc]
    private void CheckForWinners_TRPC(NetworkConnectionToClient target)
    {
        if (m_gamemasterScore == m_maxScore) 
        {
            ShowWinners_CMD(ETeam.gameMaster);
        }
        else if (m_runnerScore == m_maxScore)
        {
            ShowWinners_CMD(ETeam.runner);
        }
    }

    [Command(requiresAuthority = false)]
    private void ShowWinners_CMD(ETeam team)
    {
        ShowWinners_RPC(team);
    }

    [ClientRpc]
    private void ShowWinners_RPC(ETeam team)
    {
        string winText;

        if (team == ETeam.gameMaster)
            winText = "GameMaster";
        else
            winText = "Runner";

        m_gameOverPanel.SetActive(true);
        m_winnerText.text = winText + " wins!";
        // TODO Desactivate Script that players cant run anymore
        Time.timeScale = 0;
    }

   


   

   

}
