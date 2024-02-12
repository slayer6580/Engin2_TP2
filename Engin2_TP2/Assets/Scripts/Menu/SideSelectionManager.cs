using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectionManager : NetworkBehaviour
{
	[Header("Base Setting")]
	[SerializeField] private List<GameObject> m_enterRoomSpawnPoints = new List<GameObject>();
	[SerializeField] private int m_maxGameMaster;

	[Header ("Set all slots")]
	[SerializeField] private List<GameObject> m_selectSlotButtons = new List<GameObject>();
	[SerializeField] private List<Transform> m_slotCharacterPosition = new List<Transform>();
	[SyncVar][SerializeField] private List<bool> m_isSlotFree = new List<bool>();
	[SyncVar] private List<bool> m_isSlotReady = new List<bool>();
	[SerializeField] private List<GameObject> m_playerInThisSlotReadyText = new List<GameObject>();

	[Header("UI")]
	[SerializeField] private GameObject m_readyButton;
	[SerializeField] private float m_readyButtonSpacing;
	[SerializeField] private GameObject m_readyButtonText;

	//Misc..
	private int m_nbOfPlayer;
	private NetworkRoomManager m_networkRoomManager;		//NotDestroyOnLoad, give access to list of players, use to pass value to gameSscene
	private NetworkRoomPlayer m_localNetworkRoomPlayer;     //For network setting: Player is ready to start or not
	private int m_slotSelected = -1;
	private bool m_canClick = true;		//To avoid clicking on slot after clicking on disconnecting button
	private NetworkIdentity m_networkIdentity;
	private bool m_hasOneGameMaster;
	private bool m_hasOneRunner;

	void Awake()
	{
		m_networkIdentity = GetComponent<NetworkIdentity>();
		m_networkRoomManager = FindObjectOfType<NetworkRoomManager>();

		//Set m_isSlotReady the same as the number of slot available
		foreach (var slot in m_isSlotFree)
		{
			m_isSlotReady.Add(false);
		}	
	}

	public void Start()
	{
		//When entering room,manage active and inactive slot.	
		int i = 0;
		foreach(bool slot in m_isSlotFree)
		{
			//If a slot is taken
			if (!slot)
			{
				m_selectSlotButtons[i].SetActive(false);
				m_playerInThisSlotReadyText[i].SetActive(true);
				if (m_isSlotReady[i])
				{
					//The player in this slot is ready so change the text
					m_playerInThisSlotReadyText[i].GetComponent<TMP_Text>().text = "Ready";
				}

			}
			else
			{
				m_playerInThisSlotReadyText[i].SetActive(false);
			}
			i++;
		}
	}

	public void Update()
	{
		
		//Place a character at a spawn point in the center of the screen when a new player enter the room
		if (m_nbOfPlayer != m_networkRoomManager.roomSlots.Count)
		{
			m_nbOfPlayer = m_networkRoomManager.roomSlots.Count;

			//If the character is in the middle the "m_slotSelected" will be -1
			if (m_slotSelected < 0)
			{
				m_networkRoomManager.roomSlots[m_nbOfPlayer - 1].transform.position = m_enterRoomSpawnPoints[m_nbOfPlayer - 1].transform.position;
				m_networkRoomManager.roomSlots[m_nbOfPlayer - 1].transform.localScale = m_enterRoomSpawnPoints[m_nbOfPlayer - 1].transform.localScale;
				m_networkRoomManager.roomSlots[m_nbOfPlayer - 1].transform.eulerAngles = m_enterRoomSpawnPoints[m_nbOfPlayer - 1].transform.eulerAngles;
			}
		}
	}

	[Command(requiresAuthority = false)]
	public void ChangeNameCommand(NetworkRoomPlayer player, string newName)
	{
		ChangeNameClient(player, newName);
	}

	[ClientRpc]
	public void ChangeNameClient(NetworkRoomPlayer player, string newName)
	{
		player.gameObject.transform.name = newName;
	}

	public void SelectSlot(int wantedSlot)
	{
		if (m_canClick)
		{
			CmdGetFreeSlot(wantedSlot, m_networkIdentity.connectionToClient);
		}	
	}

	public void ValidateSlot(int wantedSlot)
	{
		//Place the character into the chosen slot
		GetLocalNetworkRoomPlayer().transform.position = m_slotCharacterPosition[wantedSlot].position;
		m_readyButton.SetActive(true);
		m_readyButton.transform.position = new Vector3(m_slotCharacterPosition[wantedSlot].position.x, m_slotCharacterPosition[wantedSlot].position.y - m_readyButtonSpacing, m_slotCharacterPosition[wantedSlot].position.z);
		ManageIsReadyTextCommand(wantedSlot, true, false);
		
		//Replace the ready text over the slot imediatly for the local player
		ManageIsReadyText(m_slotSelected, false, false);
		ManageIsReadyText(wantedSlot, true, false);
		

		//Set which side has been choosen
		if (wantedSlot >= m_maxGameMaster)
		{
			//GetLocalNetworkRoomPlayer().gameObject.GetComponent<SaveLocalPlayer>().m_isGameMaster = false;
			ChangeNameCommand(GetLocalNetworkRoomPlayer(), "Runner");

		}
		else
		{
			//GetLocalNetworkRoomPlayer().gameObject.GetComponent<SaveLocalPlayer>().m_isGameMaster = true;
			ChangeNameCommand(GetLocalNetworkRoomPlayer(), "GameMaster");

		}

		///Reset some values		
		//If already had a slot selected, reactivate it
		if (m_slotSelected >= 0)
		{
			SwitchSlotStateCommand(m_slotSelected, true);
			//Set the local Ready button to the "not ready" values
			SetAsReady(false, "Ready");
		}

		//Deactivate the new selected slot
		//SwitchSlotStateCommand(wantedSlot, false);

		//The players has change his position so he's not ready anymore
		ManageIsReadyTextCommand(m_slotSelected, false, false);

		m_slotSelected = wantedSlot;
	}

	public void ClientLeavingLobby()
	{
		if (m_canClick && m_slotSelected >= 0)
		{
			m_canClick = false;
			ManageIsReadyTextCommand(m_slotSelected, false, false);
			SwitchSlotStateCommand(m_slotSelected, true);
		}
			CmdDoneDisconnecting(m_networkIdentity.connectionToClient);
		
			
	}

	[Command(requiresAuthority = false)]
	public void CmdDoneDisconnecting(NetworkConnectionToClient target)
	{
		TargetValidateDisconnection(target);
	}

	[TargetRpc]
	public void TargetValidateDisconnection(NetworkConnectionToClient target)
	{
		BackToMainMenu();
	}

	public void BackToMainMenu()
	{
		NetworkManager.singleton.gameObject.transform.GetChild(0).GetComponent<MenuButton>().ToMainMenu();
	}

	//Toggle the ready button under the local player to set If ready or not to start le match
	public void ToggleReadyButton()
	{
		if (m_canClick)
		{
			//Check if already Ready
			if (GetLocalNetworkRoomPlayer().readyToBegin)
			{
				SetAsReady(false, "Ready");
			}
			else
			{
				SetAsReady(true, "Cancel");
			}
		}
       
	}

	public void SetAsReady(bool setToReady, string textInReadyButton)
	{
		//Change text from the local Ready/Cancel button
		m_readyButtonText.GetComponent<TMP_Text>().text = textInReadyButton;

		//Tell the server if player is ready or not
		GetLocalNetworkRoomPlayer().CmdChangeReadyState(setToReady);

		SetIfSlotIsReadyCommand(m_slotSelected, setToReady);

		//Change the Ready/Not Ready text over the player
		ManageIsReadyTextCommand(m_slotSelected, true, setToReady);
	}


	//To Change the text over the players to notify other player if ready or not
	[Command(requiresAuthority = false)]
	public void ManageIsReadyTextCommand(int slotPosition, bool newState, bool isReady)
	{
		RcpManageIsReadyText(slotPosition, newState, isReady);
	}

	[ClientRpc]
	public void RcpManageIsReadyText(int slotPosition, bool newState, bool isReady)
	{
		ManageIsReadyText(slotPosition, newState, isReady);	
	}

	public void ManageIsReadyText(int slotPosition, bool newState, bool isReady)
	{
		if (slotPosition >= 0)
		{
			TMP_Text textMesh = m_playerInThisSlotReadyText[slotPosition].GetComponent<TMP_Text>();
			m_playerInThisSlotReadyText[slotPosition].SetActive(newState);

			if (isReady)
			{
				textMesh.text = "Ready";
			}
			else
			{
				textMesh.text = "Not Ready";
			}
		}
	}

	//Toggle the grey area section to select the TEAM
	[Command(requiresAuthority = false)]
	public void SwitchSlotStateCommand(int slot, bool state)
	{
		m_isSlotFree[slot] = state;

		bool atLeastOneGameMaster = false;
		bool atLeastOneRunner = false;

		
		int i = 0;
		foreach(bool currentSlot in m_isSlotFree)
		{
			if (i < 2 && currentSlot == false)
			{
				atLeastOneGameMaster = true;
			}
			if (i >= 2 && currentSlot == false)
			{
				atLeastOneRunner = true;
			}
			i++;
		}

		if(atLeastOneGameMaster && atLeastOneRunner)
		{
			NetworkManager.singleton.HasOneInEachTeam = true;
		}
		else
		{
			NetworkManager.singleton.HasOneInEachTeam = false;
		}
	

		SwitchSlotState(slot, state);
	}
	[ClientRpc]
	public void SwitchSlotState(int slot, bool state)
	{
		m_selectSlotButtons[slot].SetActive(state);
		m_isSlotFree[slot] = state;
	}


	//Change if the player in a slot is ready or not
	[Command(requiresAuthority = false)]
	public void SetIfSlotIsReadyCommand(int slot, bool state)
	{
		SetIfSlotIsReady(slot, state);
	}
	[ClientRpc]
	public void SetIfSlotIsReady(int slot, bool state)
	{
		m_isSlotReady[slot] = state;
	}

	[Command(requiresAuthority = false)]
	public void CmdGetFreeSlot(int slot, NetworkConnectionToClient target)
	{
		
		TargetValidateSlot(target, slot, m_isSlotFree[slot]);
		SwitchSlotStateCommand(slot, false);
	}

	[TargetRpc]
	public void TargetValidateSlot(NetworkConnectionToClient target,int slot,  bool isFree)
	{
		if(isFree)
		{
			ValidateSlot(slot);
		}
		else
		{
			print("ALREADY TAKEN");
		}
		
	}

	public NetworkRoomPlayer GetLocalNetworkRoomPlayer()
	{
		if (m_localNetworkRoomPlayer == null)
		{
			m_localNetworkRoomPlayer = NetworkClient.localPlayer.gameObject.GetComponent<NetworkRoomPlayer>();
		}
		return m_localNetworkRoomPlayer;
	}

}
