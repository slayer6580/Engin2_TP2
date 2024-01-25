using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectionManager : NetworkBehaviour
{
	[SerializeField] private int m_maxNumberOfGameMaster;
	[SerializeField] private RoomManager m_roomManager;

	[Header ("Set all slots")]
	[SerializeField] private List<GameObject> m_selectionSlots = new List<GameObject>();
	[SerializeField] private List<Transform> m_selectionPos = new List<Transform>();
	[SyncVar][SerializeField] private List<bool> m_isSlotFree = new List<bool>();
	[SerializeField] private List<GameObject> m_isReadyText = new List<GameObject>();

	[Header("UI")]
	[SerializeField] private GameObject m_readyButton;
	[SerializeField] private float m_readyButtonSpacing;
	[SerializeField] private GameObject m_readyButtonText;


	private RoomPlayer m_roomPlayer;					//Custom script to record player choice: master/Runners, position in lobby
	private NetworkRoomPlayer m_networkRoomPlayer;		//For network setting: PLayer is ready to start or not


	public void Start()
	{
		//When entering room, if a slot is already taken, deactivate the visual of the slot
		int i = 0;
		foreach(bool slot in m_isSlotFree)
		{
			if (!slot)
			{
				m_selectionSlots[i].SetActive(false);
			}
			i++;
		}
	}


	public void SelectSlot(int slot)
	{
		//If the selected slot is available
		if(GetFreeSlot(slot))
		{
			if (m_roomPlayer == null)
			{
				m_roomPlayer = NetworkClient.localPlayer.gameObject.GetComponent<RoomPlayer>();
			}
		
			//Place the character into the chosen slot
			m_roomPlayer.transform.position = m_selectionPos[slot].position;
			m_readyButton.SetActive(true);
			m_readyButton.transform.position = new Vector3(m_selectionPos[slot].position.x, m_selectionPos[slot].position.y - m_readyButtonSpacing, m_selectionPos[slot].position.z);
			

			//Set which side has been choosen
			if (slot >= m_maxNumberOfGameMaster)
			{
				m_roomPlayer.SetIfGameMaster(false);
			}
			else
			{
				m_roomPlayer.SetIfGameMaster(true);
			}

			//Player has selected a new slot so reset old values (Ready button, reactivate old slot, etc..)
			if (m_roomPlayer.m_slotSelected >= 0)
			{
				// >0 means players was already in a slot. -1 = was in the center of the screen
				ToggleButtonCommand(m_roomPlayer.m_slotSelected, true);
			}
			ToggleButtonCommand(slot, false);
			ManageIsReadyTextCommand(m_roomPlayer.m_slotSelected, false, false);
			
			//If the player was ready, set it has not ready
			if (GetNewWorkRoomPlayer().readyToBegin)
			{
				ToggleReady();
			}

			//Set everything for the new slot
			m_roomPlayer.m_slotSelected = slot;
			ManageIsReadyTextCommand(slot, true, false);			
		}
	}

	
	//The ready button under the local player to set If ready or not to start le match
	public void ToggleReady()
	{	
        //Check if already Ready
		if (GetNewWorkRoomPlayer().readyToBegin)
        {
			m_readyButtonText.GetComponent<TMP_Text>().text = "Ready";
			GetNewWorkRoomPlayer().CmdChangeReadyState(false);
			ManageIsReadyTextCommand(m_roomPlayer.m_slotSelected, true, false);
		}
		else
		{
			m_readyButtonText.GetComponent<TMP_Text>().text = "Cancel";
			GetNewWorkRoomPlayer().CmdChangeReadyState(true);
			ManageIsReadyTextCommand(m_roomPlayer.m_slotSelected, true, true);
		}
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
			TMP_Text textMesh = m_isReadyText[slotPosition].GetComponent<TMP_Text>();
			m_isReadyText[slotPosition].SetActive(newState);

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
	public void ToggleButtonCommand(int slot, bool state)
	{
		ToggleButton(slot, state);
	}
	[ClientRpc]
	public void ToggleButton(int slot, bool state)
	{
		m_selectionSlots[slot].SetActive(state);
		m_isSlotFree[slot] = state;
	}


	public bool GetFreeSlot(int slot)
	{
		return m_isSlotFree[slot];
	}

	public NetworkRoomPlayer GetNewWorkRoomPlayer()
	{
		if (m_networkRoomPlayer == null)
		{
			m_networkRoomPlayer = NetworkClient.localPlayer.gameObject.GetComponent<NetworkRoomPlayer>();
		}
		return m_networkRoomPlayer;
	}

}
