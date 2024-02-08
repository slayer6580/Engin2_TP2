using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(NetworkIdentity))]
public class TerrainLever : NetworkBehaviour, IInteractable
{
	[SerializeField] private ObstacleManager m_obstacleManager;	
	[SerializeField] private NetworkIdentity m_netId;
	[SerializeField] private float m_rotationLimit;
	[SerializeField] private float m_leverMovementSpeed;
	[SerializeField] private float m_backInPlaceSpeed;

	[SerializeField] private UnityEvent m_GoUP;
	[SerializeField] private UnityEvent m_GoDown;
	[SerializeField] private UnityEvent m_toCallIfFree;
	[SerializeField] private UnityEvent m_toReleaseObstacle;

	private float currentRotation;
	private bool m_isHoldingOn;
	private float m_staminaCost;

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
		if (m_toReleaseObstacle != null)
		{
			m_obstacleManager.ReleaseObstacle();
		}
	}

	public void FreeToUse()
	{
		m_isHoldingOn = true;
	}

	public void ReleaseObstacle()
	{
		m_isHoldingOn = false;
		GmStaminaManager.GetInstance().StopOverTimeCostCommand();
	}

	void Update()
    {
		if (m_isHoldingOn)
		{
			//Make the lever up or down
			float mouseMovement = Input.GetAxis("Mouse Y");
			transform.Rotate(Vector3.right, mouseMovement * m_leverMovementSpeed * Time.deltaTime);

			//Limit the rotation of the lever
			currentRotation = GetAccurateRotationValue(transform.localEulerAngles.x);
			float clampedRotation = Mathf.Clamp(currentRotation, -m_rotationLimit, m_rotationLimit);
			transform.localEulerAngles = new Vector3(clampedRotation, 0, 0);

			//Called method to move the terrain if the lever is at it's limit position
			if (currentRotation >= m_rotationLimit)
			{
				m_GoUP.Invoke();
			}
			else if (currentRotation <= -m_rotationLimit)
			{
				m_GoDown.Invoke();
			}
			
		}
		else
		{		
			//Recenter the lever
			currentRotation = GetAccurateRotationValue(transform.localEulerAngles.x);		
			if (currentRotation < -5)
			{
				transform.Rotate(Vector3.right, m_backInPlaceSpeed * Time.deltaTime);
			}
			else if(currentRotation > 5)
			{
				transform.Rotate(Vector3.left, m_backInPlaceSpeed * Time.deltaTime);
			}
		}
    }

	//Be sure the angle is between  -180 et 180 degre
	private float GetAccurateRotationValue(float rotationToClamp)
	{

		float clampedRotation = rotationToClamp;
		if (clampedRotation > 180.0f)
		{
			clampedRotation -= 360.0f;
		}
		return clampedRotation;
	}
}
