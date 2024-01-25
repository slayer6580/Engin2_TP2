using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : NetworkBehaviour
{
    public NetworkRoomManager m_currentRoomManager;
    public List<GameObject> m_spawnPoints = new List<GameObject>();
   
    public int m_nbOfPlayer;
    public int m_placedPlayers;

	void Awake()
	{
		m_currentRoomManager = FindObjectOfType<NetworkRoomManager>();
	}

	void Update()
    {
     if(m_currentRoomManager != null)
        {
            if(m_nbOfPlayer != m_currentRoomManager.roomSlots.Count)
            {
                m_nbOfPlayer = m_currentRoomManager.roomSlots.Count;
			}
    
            if(m_placedPlayers < m_nbOfPlayer)
            {
                m_currentRoomManager.roomSlots[m_placedPlayers].transform.position = m_spawnPoints[m_placedPlayers].transform.position;
                m_currentRoomManager.roomSlots[m_placedPlayers].transform.localScale = m_spawnPoints[m_placedPlayers].transform.localScale;
                m_currentRoomManager.roomSlots[m_placedPlayers].transform.eulerAngles = m_spawnPoints[m_placedPlayers].transform.eulerAngles;
                m_placedPlayers++;
			}
            if(m_placedPlayers > m_nbOfPlayer)
            {
				m_placedPlayers--;
			}
		}   
    }
}
