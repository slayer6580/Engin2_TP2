using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(NetworkIdentity))]
public class ObstacleManager : NetworkBehaviour
{
	public float m_staminaCost;
	[SerializeField] private List<GameObject> m_changingColorObject;
	[SerializeField] private Color m_baseRed;
	[SerializeField] private Color m_inUseColor;

	private UnityEvent m_toCallIfFree;
	private UnityEvent m_toReleaseObstacle;
	private bool m_isBeingUsed;
	private NetworkIdentity m_netId;

	public void Start()
	{
		m_netId = this.gameObject.GetComponent<NetworkIdentity>();
	}

	//Be sure that nobody is already using this obstacle
	public void CheckIfFreeToUse(UnityEvent toCallIfFree, UnityEvent toReleaseObstacle)
	{
		m_toCallIfFree = toCallIfFree;
		m_toReleaseObstacle = toReleaseObstacle;

		IsFreeToUseCommand(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void IsFreeToUseCommand(NetworkConnectionToClient target)
	{
		if (m_isBeingUsed == false)
		{ 
			SetIsBeingUsedCommand(true);
			TargetWasFreeToUseClient(target);
		}
	}

	[TargetRpc]
	public void TargetWasFreeToUseClient(NetworkConnectionToClient target)
	{
		m_toCallIfFree.Invoke();	
	}


	//Called when the player release the mouse button or when there's no more stamina
	public void ReleaseObstacle()
	{
		ReleaseObstacleCommand(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void ReleaseObstacleCommand(NetworkConnectionToClient target)
	{
		SetIsBeingUsedCommand(false);
		TargetReleaseObstacle(target);
	}

	[TargetRpc]
	public void TargetReleaseObstacle(NetworkConnectionToClient target)
	{
		print("IS THIS THE BUG???!");
		ReleaseObstacleLocal();
	}

	public void ReleaseObstacleLocal()
	{
		m_toReleaseObstacle.Invoke();
	}

	//
	[Command(requiresAuthority = false)]
	public void SetIsBeingUsedCommand(bool value)
	{
		m_isBeingUsed = value;
		ChangeColorRcp(value);
	}

	public bool GetIsBeingMove()
	{
		return m_isBeingUsed;
	}

	//Change the color for everyone to notify that the obstacle is in use
	[ClientRpc]
	public void ChangeColorRcp(bool value)
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
