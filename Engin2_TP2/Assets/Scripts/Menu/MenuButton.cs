using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	NetworkManager manager;

	public InputField m_adress;
	public InputField m_port;


	public GameObject m_portActive;

	void Awake()
	{
		manager = GetComponent<NetworkManager>();
		m_adress.text = manager.networkAddress;

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
		manager.networkAddress = m_adress.text;
	}

	public void UpdatePort()
	{
		if (Transport.active is PortTransport portTransport)
		{
			// use TryParse in case someone tries to enter non-numeric characters
			if (ushort.TryParse(GUILayout.TextField(portTransport.Port.ToString()), out ushort port))
				portTransport.Port = port;

			//Can't test with port if problem with TryParse,maybe try with this...
			//ushort port = Convert.ToUInt16(m_port.text);
			//portTransport.Port = port;
		}
	}

	public void StartHostButton()
	{
		manager.StartHost();
	}

	public void StartJoinButton()
	{
		manager.StartClient();
	}

}
