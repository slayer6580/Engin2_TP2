using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;
using UnityEngine.Events;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]

public class ActivateObstacle : NetworkBehaviour, IInteractable
{

	[SerializeField] private ObstacleManager m_obstacleManager;

	
    [SerializeField] private bool m_isInstantCost;

    [SerializeField] private UnityEvent m_toCallIfFree;
	[SerializeField] private UnityEvent m_toReleaseObstacle;



	public void OnPlayerClicked(GameMasterController player)
	{
		if (GmStaminaManager.GetInstance().CanUseStaminaOverTime(m_obstacleManager.m_staminaCost))
		{
			if (m_toCallIfFree != null)
			{
				m_obstacleManager.CheckIfFreeToUse(m_toCallIfFree, m_toReleaseObstacle);
			}
		}
		else
		{
			print("ERROR: NOT ENOUGH STAMINA!!! !! ! !!");

		}
			

	}

	public void OnPlayerClickUp(GameMasterController player)
	{
		if(m_toReleaseObstacle != null)
		{
			m_obstacleManager.ReleaseObstacle();
		}		
	}




}
