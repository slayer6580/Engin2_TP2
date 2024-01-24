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

	public Text m_adress;
	public Text m_port;

	public GameObject m_portActive;

	void Awake()
	{
		manager = GetComponent<NetworkManager>();
		m_adress.text = manager.networkAddress;
		//manager.networkAddress = GUILayout.TextField(manager.networkAddress);

		m_adress.text = "TEST";	//Pourquoi ça marche pas??

		//Port
		if (Transport.active is PortTransport portTransport)
		{
			//?????? Marche pas non plus...
			//use TryParse in case someone tries to enter non-numeric characters
			//if (ushort.TryParse(m_port.text), out ushort port)
			//portTransport.Port = port;
			
			ushort port = Convert.ToUInt16(m_port.text);
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
			//?????? Marche pas non plus...
			//use TryParse in case someone tries to enter non-numeric characters
			//if (ushort.TryParse(m_port.text), out ushort port)
			//portTransport.Port = port;

			ushort port = Convert.ToUInt16(m_port.text);
			portTransport.Port = port;
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
