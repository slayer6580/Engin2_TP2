using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class RotatingObstacle : NetworkBehaviour
{
	[Header("Important")]
	[SerializeField] private ObstacleManager m_obstacleManager;
	[SerializeField] private GameObject m_toRotate;

	[Header("Settings")]
	[SerializeField] private float m_rotatingSpeed;

    private bool m_isRotating;
	private float m_staminaCost;

	public void Rotate()
    {	
		CmdRotate();
	}

	[Command(requiresAuthority = false)]
	public void CmdRotate()
	{
		m_staminaCost = m_obstacleManager.m_staminaCost;
		GmStaminaManager.GetInstance().StartOverTimeCostCommand(m_staminaCost);
		RcpRotate();
	}

	[ClientRpc]
	public void RcpRotate()
	{
		m_isRotating = true;
	}


    public void StopRotating()
    {
		CmdStopRotating();
	}

	[Command(requiresAuthority = false)]
	public void CmdStopRotating()
	{
		GmStaminaManager.GetInstance().StopOverTimeCostCommand();
		RcpStopRotating();
	}

	[ClientRpc]
	public void RcpStopRotating()
	{
		
		m_isRotating = false;
	}

	public void Update()
	{
		if (m_isRotating)
		{
			if (GmStaminaManager.GetInstance().CanUseStaminaOverTime(m_staminaCost))
			{
				m_toRotate.transform.Rotate(Vector3.up, m_rotatingSpeed * Time.deltaTime);
			}
			else
			{
				m_obstacleManager.ReleaseObstacle();
			}
		}
	}
}
