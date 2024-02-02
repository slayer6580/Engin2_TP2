using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
	[SerializeField] private Animator m_trapAnimator;
	[SerializeField] private float m_timeOpen;
	[SerializeField] private float m_timeToResetAfterClose;
	private float m_currentTimer;
	private bool m_isOpen;
	private bool m_isWaitingToReset;
	private bool m_canBeClickedOn = true;


	public void ActivateTrap()
	{
		if (m_canBeClickedOn)
		{
			m_canBeClickedOn = false;
			m_trapAnimator.SetTrigger("Open");
			m_isOpen = true;
			m_currentTimer = m_timeOpen;
		}	
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
				m_trapAnimator.SetTrigger("Close");
				m_currentTimer = m_timeToResetAfterClose;
				m_isWaitingToReset = true;
			}
		}

		if (m_isWaitingToReset)
		{
			m_currentTimer -= Time.deltaTime;
			if (m_currentTimer <= 0)
			{
				m_isWaitingToReset = false;
				m_trapAnimator.SetTrigger("Reset");
				m_canBeClickedOn = true;
			}
		}
	}

}
