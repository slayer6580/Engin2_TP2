using Mirror;
using TMPro;
using UnityEngine;
using System.Collections;

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
    [Header("Dans le canvas du GameManager")]
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private TextMeshProUGUI m_winnerText;
    [SerializeField] private TextMeshProUGUI m_timeText;
    [Header("Le score a atteindre pour finir la partie")]
    [SerializeField] private int m_maxScore;
    [Header("Le nombre de secondes restants pour retourner au menu une fois la partie terminé")]
    [SerializeField] private int m_secondBeforeExit;

    private int m_gamemasterScore = 0;
    private int m_runnerScore = 0;
    private GameObject m_localPlayer;

    private static ScoreManager s_instance = null;

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


    /// <summary> Pour dire que le score a augmenter pour une équipe </summary>
    [Command(requiresAuthority = false)]
    public void CmdUpdateScore(ETeam team, NetworkIdentity player)
    {
        RpcUpdateScore(team);
        TargetCheckForWinners(player.connectionToClient);
    }


    /// <summary> Pour augmenter le score d'une équipe et le gerer dans le UI </summary>
    [ClientRpc]
    private void RpcUpdateScore(ETeam team)
    {
        if (team == ETeam.gameMaster)
            m_gamemasterScore++;
        else
            m_runnerScore++;

        m_gamemasterScoreText.text = m_gamemasterScore.ToString();
        m_runnerScoreText.text = m_runnerScore.ToString();

    }

    /// <summary> Pour regarder si la partie est finie </summary>
    [TargetRpc]
    private void TargetCheckForWinners(NetworkConnectionToClient target)
    {
        if (m_gamemasterScore == m_maxScore)
        {
            CmdShowWinners(ETeam.gameMaster);
        }
        else if (m_runnerScore == m_maxScore)
        {
            CmdShowWinners(ETeam.runner);
        }
    }

    /// <summary> Pour dire que la partie es finie </summary>
    [Command(requiresAuthority = false)]
    private void CmdShowWinners(ETeam team)
    {
        RpcShowWinners(team);
    }

    /// <summary> Montrer le UI comme quoi la partie es finie et arreter le jeu </summary>
    [ClientRpc]
    private void RpcShowWinners(ETeam team)
    {
        string winText;

        if (team == ETeam.gameMaster)
            winText = "GameMaster";
        else
            winText = "Runner";

        m_gameOverPanel.SetActive(true);
        m_winnerText.text = winText + " wins!";
        m_timeText.text = m_secondBeforeExit.ToString();

        DesactivateEndGameComponents();
        StartCoroutine(WaitToQuitGame());
    }

    void DesactivateEndGameComponents()
    {
        m_localPlayer.GetComponent<PlayerSetup>().DesactivateEndGameComponents();
    }

    public void SetLocalPlayer(NetworkIdentity clientObject)
    {
        m_localPlayer = clientObject.gameObject;
    }

    IEnumerator WaitToQuitGame()
    {
        while (m_secondBeforeExit > 0)
        {
            yield return new WaitForSeconds(1);
            m_secondBeforeExit--;
            m_timeText.text = m_secondBeforeExit.ToString();
        }

        m_localPlayer.GetComponent<PlayerPause>().Lobby();
    }


}
