using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTeamManager : NetworkBehaviour
{
    private GameObject m_localPlayer;
    [SerializeField] private List<MeshRenderer> m_toRemoveFromRunnerPlayers = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> m_toRemoveFromGameMasters = new List<MeshRenderer>();
    // Start is called before the first frame update
    void Start()
    {
      
    }

	private void Update()
	{
        if (m_localPlayer == null)
        {
			m_localPlayer = NetworkClient.localPlayer.gameObject;

			if (m_localPlayer == null)
            {
                print("NOT FOUND");
                return;
			}

			if (NetworkClient.localPlayer.gameObject.GetComponent<PlayerStateMachine>() != null)
			{
				//Runner
				foreach (MeshRenderer obj in m_toRemoveFromRunnerPlayers)
				{
					obj.enabled = false;
				}
			}
			else
			{
				//GameMaster
				foreach (MeshRenderer obj in m_toRemoveFromGameMasters)
				{
					obj.enabled = false;
				}
			}
			
		}	
	}
}
