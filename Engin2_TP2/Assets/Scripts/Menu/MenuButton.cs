using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : NetworkBehaviour
{

	[SerializeField] private InputField m_adress;
	[SerializeField] private InputField m_port;
	[SerializeField] private GameObject m_portActive;

	void Start()
	{
		m_adress.text = NetworkManager.singleton.networkAddress;

		//Port
		if (Transport.active is PortTransport portTransport)
		{
			// use TryParse in case someone tries to enter non-numeric characters
			if (ushort.TryParse(GUILayout.TextField(portTransport.Port.ToString()), out ushort port))
				portTransport.Port = port;
		}
		else
		{
			m_portActive.SetActive(false);
		}
	}

	public void UpdateIp()
	{
		NetworkManager.singleton.networkAddress = m_adress.text;
	}

	public void UpdatePort()
	{
		if (Transport.active is PortTransport portTransport)
		{
			// use TryParse in case someone tries to enter non-numeric characters
			if (ushort.TryParse(m_port.text, out ushort port))
			{
				portTransport.Port = port;			
			}
			m_port.text = port.ToString();
		}
	}

	public void StartHostButton()
	{
		NetworkManager.singleton.StartHost();
	}

	public void StartJoinButton()
	{
		NetworkManager.singleton.StartClient();
	}

	//Back
	public void ToMainMenu()
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
