using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(NetworkIdentity))]
public class ObstacleManager : NetworkBehaviour
{
	private bool m_isBeingUsed;
	private NetworkIdentity m_netId;
	[SerializeField] private float m_staminaCost;
	[SerializeField] private List<GameObject> m_changingColorObject;
	[SerializeField] private Color m_baseRed;
	[SerializeField] private Color m_inUseColor;

	private UnityEvent m_toCallIfFree;
	private UnityEvent m_toReleaseObstacle;

	public void Start()
	{
		m_netId = this.gameObject.GetComponent<NetworkIdentity>();
	}

	public void CheckIfFreeToUse(UnityEvent toCallIfFree)
	{
		m_toCallIfFree = toCallIfFree;
		IsFreeToUseCommand(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void IsFreeToUseCommand(NetworkConnectionToClient target)
	{
		if (m_isBeingUsed == false)
		{
			SetIsBeingUsed(true);
			TargetWasFreeToUseClient(target);
		}
	}

	[TargetRpc]
	public void TargetWasFreeToUseClient(NetworkConnectionToClient target)
	{
		m_toCallIfFree.Invoke();
		
	}


	public void ReleaseObstacle(UnityEvent toReleaseObstacle)
	{
		m_toReleaseObstacle = toReleaseObstacle;
		ReleaseObstacleCommand(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void ReleaseObstacleCommand(NetworkConnectionToClient target)
	{
		SetIsBeingUsed(false);
		TargetReleaseObstacle(target);
	}

	[TargetRpc]
	public void TargetReleaseObstacle(NetworkConnectionToClient target)
	{
		m_toReleaseObstacle.Invoke();
	}




	//This is and must always called by the server only
	public void SetIsBeingUsed(bool value)
	{
		m_isBeingUsed = value;
		ChangeColor(value);
	}

	public bool GetIsBeingMove()
	{
		return m_isBeingUsed;
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
