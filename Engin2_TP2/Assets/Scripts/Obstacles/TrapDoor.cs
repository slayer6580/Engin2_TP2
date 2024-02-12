using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

[RequireComponent(typeof(NetworkIdentity))]

public class TrapDoor : NetworkBehaviour
{
	[Header("Important")]
	[SerializeField] private ObstacleManager m_obstacleManager;
	[SerializeField] private Animator m_trapAnimator;

	[Header("Settings")]
	[SerializeField] private float m_timeOpen;
	[SerializeField] private float m_timeToResetAfterClose;

	private float m_currentTimer;
	private bool m_isOpen;
	private bool m_isWaitingToReset;
	[SyncVar]private bool m_canBeClickedOn = true;
	private float m_staminaCost;

	public void ActivateTrap()
	{
		if (m_canBeClickedOn)
		{		
			ActivateTrapCommand();
		}	
	}

	[Command(requiresAuthority = false)]
	public void ActivateTrapCommand()
	{
		AudioManager.GetInstance().CmdPlaySoundEffectsOneShotAll(ESound.trap, gameObject.transform.position);
		m_staminaCost = m_obstacleManager.m_staminaCost;
		GmStaminaManager.GetInstance().InstantCostCommand(m_staminaCost);
		m_canBeClickedOn = false;
		m_isOpen = true;
		m_currentTimer = m_timeOpen;
		ActivateTrapRPC();
	}

	[ClientRpc]
	public void ActivateTrapRPC()
	{
		m_canBeClickedOn = false;
		m_trapAnimator.SetTrigger("Open");
	}

	[ClientRpc]
	public void CloseTrap()
	{
		m_trapAnimator.SetTrigger("Close");
	}

	[ClientRpc]
	public void ResetTrap()
	{
		m_trapAnimator.SetTrigger("Reset");
		m_canBeClickedOn = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_isOpen)
		{
			m_currentTimer -= Time.deltaTime;
			if (m_currentTimer <= 0)
			{
				m_isOpen = false;			
				m_currentTimer = m_timeToResetAfterClose;
				m_isWaitingToReset = true;
				CloseTrap();
			}
		}

		if (m_isWaitingToReset)
		{
			m_currentTimer -= Time.deltaTime;
			if (m_currentTimer <= 0)
			{
				m_isWaitingToReset = false;
				ResetTrap();
			}
		}
	}

}
