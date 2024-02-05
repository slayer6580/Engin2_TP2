using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ArenaRotationManager : NetworkBehaviour
{
	private bool m_isBeingMoved;

	[SerializeField] private List<GameObject> m_changingColorObject;
	[SerializeField] private Color m_baseRed;
	[SerializeField] private Color m_inUseColor;

	public void SetIsBeingMoved(bool value)
	{
		m_isBeingMoved = value;
		ChangeColor(value);
	}

	public bool GetIsBeingMove()
	{
		return m_isBeingMoved;
	}
	[ClientRpc]
	public void ChangeColor(bool value)
	{
		if (value)
		{
			foreach (GameObject obj in m_changingColorObject)
			{
				obj.GetComponent<MeshRenderer>().material.color = m_inUseColor;
			}
		}
		else
		{
			foreach (GameObject obj in m_changingColorObject)
			{
				obj.GetComponent<MeshRenderer>().material.color = m_baseRed;
			}
		}
	}

	
}
