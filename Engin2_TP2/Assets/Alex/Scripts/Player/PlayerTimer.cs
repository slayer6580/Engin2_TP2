using Mirror;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterSpawnPoint))]
public class PlayerTimer : NetworkBehaviour
{
    [SerializeField] private float m_secondsToCompleteLevel;
    [SerializeField] TextMeshProUGUI m_timeText;

    private float m_currentTime;
    private CharacterSpawnPoint m_characterSpawnPoint;

    private void Awake()
    {
        ResetTimer();
        m_characterSpawnPoint = GetComponent<CharacterSpawnPoint>();    
    }

    private void Update()
    {
        m_currentTime -= Time.deltaTime;

        if (m_currentTime <= 0)
        {
            m_characterSpawnPoint.GoToSpawnPoint();
            m_currentTime = m_secondsToCompleteLevel;
        }

        m_timeText.text = "Time: " + ((int)m_currentTime).ToString();        
    }

    public void ResetTimer()
    {
        m_currentTime = m_secondsToCompleteLevel;
    }

}
