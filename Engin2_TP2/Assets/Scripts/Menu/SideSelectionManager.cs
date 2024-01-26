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


	void Awake()
	{
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

	public void SelectSlot(int wantedSlot)
	{	
		//If the selected slot is available
		if (GetFreeSlot(wantedSlot))
		{

			//Place the character into the chosen slot
			GetLocalNetworkRoomPlayer().transform.position = m_slotCharacterPosition[wantedSlot].position;
			m_readyButton.SetActive(true);
			m_readyButton.transform.position = new Vector3(m_slotCharacterPosition[wantedSlot].position.x, m_slotCharacterPosition[wantedSlot].position.y - m_readyButtonSpacing, m_slotCharacterPosition[wantedSlot].position.z);
			ManageIsReadyTextCommand(wantedSlot, true, false);

			//Set which side has been choosen
			if (wantedSlot >= m_maxGameMaster)
			{
				m_networkRoomManager.GetComponent<SaveLocalPlayer>().m_isGameMaster = false;
			}
			else
			{
				m_networkRoomManager.GetComponent<SaveLocalPlayer>().m_isGameMaster = true;
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
			SwitchSlotStateCommand(wantedSlot, false);

			//The players has change his position so he's not ready anymore
			ManageIsReadyTextCommand(m_slotSelected, false, false);
			
			m_slotSelected = wantedSlot;	
			
		}
	}

	
	//Toggle the ready button under the local player to set If ready or not to start le match
	public void ToggleReadyButton()
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
		ManageIsReadyText(slotPosition, newState, isReady);
	}

	[ClientRpc]
	public void ManageIsReadyText(int slotPosition, bool newState, bool isReady)
	{
		if (slotPosition >= 0)
		{
			TMP_Text textMesh = m_playerInThisSlotReadyText[slotPosition].GetComponent<TMP_Text>();
			m_playerInThisSlotReadyText[slotPosition].SetActive(newState);

			if (isReady)
			{	
				textMesh.text= "Ready";
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

	public bool GetFreeSlot(int slot)
	{
		return m_isSlotFree[slot];
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
