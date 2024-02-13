using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;
using static AudioManager;

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
	private NetworkConnectionToClient m_usedBy;

	public void Start()
	{
		m_netId = this.gameObject.GetComponent<NetworkIdentity>();
	}

	//Be sure that nobody is already using this obstacle
	public void CheckIfFreeToUse(UnityEvent toCallIfFree, UnityEvent toReleaseObstacle)
	{
		m_toCallIfFree = toCallIfFree;
		m_toReleaseObstacle = toReleaseObstacle;

		CmdIsFreeToUse(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void CmdIsFreeToUse(NetworkConnectionToClient target)
	{
		if (m_isBeingUsed == false)
		{
			m_usedBy = target;
			CmdSetIsBeingUsed(true);
			TargetWasFreeToUseClient(target);
		}
		else 
		{
			TargetNotFreeToUse(target);
		}
	}

	[TargetRpc]
	public void TargetWasFreeToUseClient(NetworkConnectionToClient target)
	{
		m_toCallIfFree.Invoke();	
	}

	[TargetRpc]
	public void TargetNotFreeToUse(NetworkConnectionToClient target)
	{
		//Play sound
		AudioManager.GetInstance().CmdPlaySoundEffectsOneShotTarget(ESound.noStamina, NetworkClient.localPlayer.gameObject.transform.position, NetworkClient.localPlayer.gameObject.GetComponent<NetworkIdentity>());
	}


	//Called when the player release the mouse button or when there's no more stamina
	public void ReleaseObstacle()
	{
		CmdReleaseObstacle(m_netId.connectionToClient);
	}

	[Command(requiresAuthority = false)]
	public void CmdReleaseObstacle(NetworkConnectionToClient target)
	{
		if(target == m_usedBy)
		{
			CmdSetIsBeingUsed(false);
			TargetReleaseObstacle(target);
		}	
	}

	[TargetRpc]
	public void TargetReleaseObstacle(NetworkConnectionToClient target)
	{
		ReleaseObstacleLocal();
	}

	public void ReleaseObstacleLocal()
	{
		m_toReleaseObstacle?.Invoke();
	}

	
	[Command(requiresAuthority = false)]
	public void CmdSetIsBeingUsed(bool value)
	{
		m_isBeingUsed = value;
		RcpChangeColor(value);
	}

	public bool GetIsBeingMove()
	{
		return m_isBeingUsed;
	}

	//Change the color for everyone to notify that the obstacle is in use
	[ClientRpc]
	public void RcpChangeColor(bool value)
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
