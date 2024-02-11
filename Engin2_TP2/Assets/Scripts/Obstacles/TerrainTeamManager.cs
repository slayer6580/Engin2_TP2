using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTeamManager : NetworkBehaviour
{
    private GameObject m_localPlayer;
    [SerializeField] private List<MeshRenderer> m_toRemoveFromRunnerPlayers = new List<MeshRenderer>();
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
				foreach (MeshRenderer obj in m_toRemoveFromRunnerPlayers)
				{
					obj.enabled = false;
				}
			}
			print("FOUND");
		}	
	}
}
