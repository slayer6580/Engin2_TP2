using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : NetworkBehaviour
{
    public NetworkRoomManager m_currentRoomManager;
    public List<GameObject> m_spawnPoints = new List<GameObject>();
    public List<GameObject> m_clientReadyText = new List<GameObject>();


    public int m_nbOfPlayer;
    private int m_placedPlayers;

	void Awake()
	{
		m_currentRoomManager = FindObjectOfType<NetworkRoomManager>();
		if (m_currentRoomManager != null)
        {

		}
	}


	[Command(requiresAuthority = false)]
	public void PlaceReadyTextCommand()
    {
        print("COMMAND CALLED");
        PlaceReadyText();

	}

	[ClientRpc] 
    public void PlaceReadyText()
	{
        int i = 0;
        foreach(NetworkRoomPlayer client in m_currentRoomManager.roomSlots)
        {
			print("PLACEMENT");
			if (client.isLocalPlayer == false)
            {	
				m_clientReadyText[i].transform.position = client.transform.position;				
			}
			i++;
		}
	}

	

	// Update is called once per frame
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
                print("PLAYER PLACED");
			}
		}   
    }
}
