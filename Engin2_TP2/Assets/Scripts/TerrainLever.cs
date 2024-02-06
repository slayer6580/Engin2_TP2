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



	private bool m_isHoldingOn;
	[SerializeField] private NetworkIdentity m_netId;
	[SerializeField] private float m_rotationLimit;
	[SerializeField] private float m_leverMovementSpeed;
	[SerializeField] private float m_backInPlaceSpeed;

	[SerializeField] private UnityEvent m_GoUP;
	[SerializeField] private UnityEvent m_GoDown;
	[SerializeField] private UnityEvent m_toCallIfFree;
	[SerializeField] private UnityEvent m_toReleaseObstacle;

	private float currentRotation;


	public void OnPlayerClicked(GameMasterController player)
	{
		m_obstacleManager.CheckIfFreeToUse(m_toCallIfFree);
		
	}
	
	public void OnPlayerClickUp(GameMasterController player)
	{
		m_obstacleManager.ReleaseObstacle(m_toReleaseObstacle);
	}

	public void FreeToUse()
	{
		m_isHoldingOn = true;
	}

	public void ReleaseObstacle()
	{
		m_isHoldingOn = false;
	}



	// Update is called once per frame
	void Update()
    {
		if (m_isHoldingOn)
		{
			float mouseMovement = Input.GetAxis("Mouse Y");


			transform.Rotate(Vector3.right, mouseMovement * m_leverMovementSpeed * Time.deltaTime);

			
			currentRotation = ClampCurrentRotation(transform.localEulerAngles.x);

			float clampedRotation = Mathf.Clamp(currentRotation, -m_rotationLimit, m_rotationLimit);
			transform.localEulerAngles = new Vector3(clampedRotation, 0, 0);


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
			currentRotation = ClampCurrentRotation(transform.localEulerAngles.x);
			
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
	private float ClampCurrentRotation(float rotationToClamp)
	{

		float clampedRotation = rotationToClamp;
		if (clampedRotation > 180.0f)
		{
			clampedRotation -= 360.0f;
		}
		return clampedRotation;
	}


	public void OnPlayerCollision(Player player)
	{
		throw new System.NotImplementedException();
	}

	public void StaminaCost(GameMasterController player)
	{
		throw new System.NotImplementedException();
	}
}
