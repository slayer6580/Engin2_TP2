using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerTeam : NetworkBehaviour
{

	[SerializeField] private GameObject m_gameMaster;
	[SerializeField] private GameObject m_runner;
	[SerializeField] private GameMasterController m_gameMasterController;

	/// <summary>
	/// //ADD GAME MASTER CAMERA TOO, MAYBE PLAYER CAM TOO..
	/// </summary>
	[SerializeField] private PlayerStateMachine m_playerStateMachine;
	public NetworkRoomManager m_roomManager;
	[SyncVar] public bool m_isGameMaster;

	private bool m_thisHaveCalledDeactivate = false;

	private void Start()
	{
		m_roomManager = FindObjectOfType<NetworkRoomManager>();

		if (NetworkClient.localPlayer.gameObject == this.gameObject)
		{
			m_isGameMaster = m_roomManager.gameObject.GetComponent<SaveLocalPlayer>().m_isGameMaster;
		}
		else
		{
			m_gameMasterController.enabled = false;
			m_playerStateMachine.enabled = false;
		}

		if (m_isGameMaster)
		{
			if (isLocalPlayer)
			{
				m_thisHaveCalledDeactivate = true;
				DeactivateThisCommand();
			}
			transform.position = Vector3.zero;
		}
	}


	[Command(requiresAuthority = false)]
	public void DeactivateThisCommand()
	{
		DeactivateThis();
	}

	[Client]
	public void DeactivateThis()
	{
		if(m_thisHaveCalledDeactivate == false)
		{
			this.gameObject.SetActive(false);
		}
		else
		{
			//If I get here that mean this is the client that initiated deactivation, so reset the bool
			m_thisHaveCalledDeactivate = false;
		}
		
	}

	void Update()
	{
		m_runner.SetActive(!m_isGameMaster);
		m_gameMaster.SetActive(m_isGameMaster);
	}


}

