using Mirror;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerCheckpoint))]
public class PlayerTimer : NetworkBehaviour
{
    [SerializeField] private float m_secondsToCompleteLevel;
    [SerializeField] TextMeshProUGUI m_timeText;

    private float m_currentTime;
    private PlayerCheckpoint m_characterSpawnPoint;

    private void Awake()
    {
        ResetTimer();
        m_characterSpawnPoint = GetComponent<PlayerCheckpoint>();    
    }

    private void Update()
    {
        m_currentTime -= Time.deltaTime;

        if (m_currentTime <= 0)
        {
            m_characterSpawnPoint.GoToSpawnPoint();
            ResetTimer();
        }

        m_timeText.text = "Time: " + ((int)m_currentTime).ToString();        
    }

    /// <summary> Reset le timer du joueur </summary>
    public void ResetTimer()
    {
        m_currentTime = m_secondsToCompleteLevel;
    }

    /// <summary> Rajouter du temps au timer du joueur </summary>
    public void AddBonusToTimer(float bonus)
    {
        m_currentTime += bonus;
    }

}
