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
		RotateCommand();
	}

	[Command(requiresAuthority = false)]
	public void RotateCommand()
	{
		AudioManager.GetInstance().CmdPlaySoundEffectsLoop(ESound.spinning, gameObject.transform.position);
		m_staminaCost = m_obstacleManager.m_staminaCost;
		GmStaminaManager.GetInstance().StartOverTimeCostCommand(m_staminaCost);
		RotateRcp();
	}

	[ClientRpc]
	public void RotateRcp()
	{
		m_isRotating = true;
	}


    public void StopRotating()
    {
		StopRotatingCommand();
	}

	[Command(requiresAuthority = false)]
	public void StopRotatingCommand()
	{
		AudioManager.GetInstance().CmdStopSoundEffectsLoop(ESound.spinning, gameObject.transform.position);
		GmStaminaManager.GetInstance().StopOverTimeCostCommand();
		StopRotatingRcp();
	}

	[ClientRpc]
	public void StopRotatingRcp()
	{
		
		m_isRotating = false;
	}


	public void Update()
	{
		if (m_isRotating)
		{	
			m_toRotate.transform.Rotate(Vector3.up, m_rotatingSpeed * Time.deltaTime);
		}
	}
}
