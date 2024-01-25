using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectionManager : NetworkBehaviour
{
	[SerializeField] private int m_maxNumberOfGameMaster;
	[SerializeField] private List<GameObject> m_selectionSlots = new List<GameObject>();
	[SerializeField] private List<Transform> m_selectionPos = new List<Transform>();
	[SyncVar][SerializeField] private List<bool> m_isSlotFree = new List<bool>();
	[SerializeField] private List<GameObject> m_isReadyText = new List<GameObject>();
	[SerializeField] private GameObject m_readyButton;
	[SerializeField] private float m_readyButtonSpacing;

	private RoomPlayer m_roomPlayer;
	[SerializeField] private RoomManager m_roomManager;
	[SerializeField] private GameObject m_readyButtonText;
	private NetworkRoomPlayer m_networkRoomPlayer;
	public void Start()
	{
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

	public void SelectSlot(int slot)
	{
		if(GetFreeSlot(slot))
		{
			if (m_roomPlayer == null)
			{
				m_roomPlayer = NetworkClient.localPlayer.gameObject.GetComponent<RoomPlayer>();
			}

			
			//Replace the character into the chosen slot
			m_roomPlayer.transform.position = m_selectionPos[slot].position;
			m_readyButton.SetActive(true);
			m_readyButton.transform.position = new Vector3(m_selectionPos[slot].position.x, m_selectionPos[slot].position.y - m_readyButtonSpacing, m_selectionPos[slot].position.z);
			

			//Set which size has been choosen
			if (slot >= m_maxNumberOfGameMaster)
			{
				m_roomPlayer.SetIfGameMaster(false);
			}
			else
			{
				m_roomPlayer.SetIfGameMaster(true);
			}

			//If the player already is in a slot
			if (m_roomPlayer.m_slotSelected >= 0)
			{
				ToggleButtonCommand(m_roomPlayer.m_slotSelected, true);
			}

			ToggleButtonCommand(slot, false);
			ManageIsReadyTextCommand(m_roomPlayer.m_slotSelected, false, false);

			m_roomPlayer.m_slotSelected = slot;
			ManageIsReadyTextCommand(slot, true, false);

			if (GetNewWorkRoomPlayer().readyToBegin)
			{
				ToggleReady();
			}
		}
		

	}

	

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



	[Command(requiresAuthority = false)]
	public void ManageIsReadyTextCommand(int toChange, bool newState, bool isReady)
	{
		ManageIsReadyText(toChange, newState, isReady);
	}
	[ClientRpc]
	public void ManageIsReadyText(int toChange, bool newState, bool isReady)
	{
		
		if (toChange >= 0)
		{
			TMP_Text textMesh;
			m_isReadyText[toChange].SetActive(newState);
			if (isReady)
			{
				textMesh = m_isReadyText[toChange].GetComponent<TMP_Text>();
				if(textMesh != null)
				{
					textMesh.text= "Ready";
				}

			}
			else
			{
				textMesh = m_isReadyText[toChange].GetComponent<TMP_Text>();
				if (textMesh != null)
				{
					textMesh.text = "Not Ready";
				}

			}
			
		}
		
	}




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




}
