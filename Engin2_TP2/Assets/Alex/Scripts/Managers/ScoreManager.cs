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

    public int m_gamemasterScore = 0;
	public int m_runnerScore = 0;

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

    public void UpdateScore(ETeam team)
    {
        UpdateScoreCommand(team);

	}


    [Command(requiresAuthority = false)]
    public void UpdateScoreCommand(ETeam team)
    {
		//UpdateScoreFinal(team);
		UpdateScoreRpc(team);
	}


    /// <summary> Pour augmenter le score d'une équipe et le gerer dans le UI </summary>

    [ClientRpc]
    private void UpdateScoreRpc(ETeam team)
    {
		UpdateScoreFinal(team);
	}

	private void UpdateScoreFinal(ETeam team)
	{
		if (team == ETeam.gameMaster)
		{
			m_gamemasterScore++;
		}
		else
		{
			m_runnerScore++;
		}
		m_gamemasterScoreText.text = m_gamemasterScore.ToString();
		m_runnerScoreText.text = m_runnerScore.ToString();
	}

}
