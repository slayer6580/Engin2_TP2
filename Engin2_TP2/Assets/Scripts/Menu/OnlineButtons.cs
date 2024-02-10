using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnlineButtons : NetworkBehaviour
{
	NetworkManager manager;
	[Scene][SerializeField] private string m_roomScene;
	[Scene][SerializeField] private string m_mainMenuScene;
	[SerializeField] private GameObject m_lobbyConfirmationPanel;

	private int m_debugCount = 1;
	void Start()
	{
		manager = FindObjectOfType<NetworkManager>();
	}

	public void RoomLobyToMainMenu()
	{
		StartCoroutine(StartExit());

	}

	IEnumerator StartExit()
	{
		
		yield return new WaitForSeconds(0.25f);

		if (isServer)
		{
			StartCoroutine(CompletExit());
		}
		
		NetworkManager.singleton.StopClient();
	}

	IEnumerator CompletExit()
	{
		yield return new WaitForSeconds(0.25f);
		NetworkManager.singleton.StopServer();
	}
}
