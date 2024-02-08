using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ArenaRotation : NetworkBehaviour
{
	enum Point { North, East, South, West };

	[Header("Important")]
	[SerializeField] private ObstacleManager m_obstacleManager;
	[SerializeField] GameObject m_arena;

	[Header("Settings")]
	[SerializeField] Point m_rotationPoint;
	[SerializeField] private float m_maxX;
	[SerializeField] private float m_minX;
	[SerializeField] private float m_movementSpeed;

	private bool m_isSelected;
	private const float ADJUST_SPEED = 100;
	private float m_staminaCost;

	[SyncVar] private Vector3 m_arenaRotation = Vector3.zero;

	//Call the good movement on the server depending of which switch has been activated
	public void CallMovement(int upOrDown)
    {
		m_staminaCost = m_obstacleManager.m_staminaCost;

		switch (m_rotationPoint)
		{
			case Point.North:
				MoveArena(upOrDown, new Vector3(1, 0, 0));
				break;

			case Point.South:
				MoveArena(upOrDown, new Vector3(1, 0, 0));
				break;

			case Point.East:
				MoveArena(upOrDown, new Vector3(0, 0, 1));
				break;

			case Point.West:
				MoveArena(upOrDown, new Vector3(0, 0, 1));
				break;
		}
	}


	[Command(requiresAuthority = false)]
	public void MoveArena(float upOrDown, Vector3 direction)
	{
		GmStaminaManager.GetInstance().StartOverTimeCostCommand(m_staminaCost);

		if (direction.x == 1)
		{
			RotateCommand(upOrDown, direction, m_arena.transform.localEulerAngles.z, true);
			//ResetAngleCommand(m_arena.transform.localEulerAngles.z, true);
		}
		else
		{
			RotateCommand(upOrDown, direction, m_arena.transform.localEulerAngles.x, false);
			//ResetAngleCommand(m_arena.transform.localEulerAngles.x, false);
		}

	}

	[Command(requiresAuthority = false)]
	public void RotateCommand(float upOrDown, Vector3 direction, float resetAngle, bool isX)
	{
		//Move the arena
		m_arena.transform.Rotate(direction, upOrDown * m_movementSpeed * Time.deltaTime);

		//Limit the movement. ClampCurrentRotation make sure the angle is between  -180 et 180 degre
		float angle = GetAccurateRotationValue((m_arena.transform.localEulerAngles.x * direction.x) + (m_arena.transform.localEulerAngles.z * direction.z));

		//Check if the angle is between the given limits
		float clampedRotation = Mathf.Clamp(angle, m_minX, m_maxX);

		//If it's been clamped, it means it got over the limits so replace it.
		if (clampedRotation == m_minX || clampedRotation == m_maxX)
		{
			m_arena.transform.localEulerAngles = new Vector3((clampedRotation * direction.x) + (m_arena.transform.localEulerAngles.x * direction.z), 0, (clampedRotation * direction.z) + (m_arena.transform.localEulerAngles.z * direction.x) );
		}

		ResetAngleCommand(resetAngle, isX);
	}


	//Be sure the angle is between -180 et 180 degre
	private float GetAccurateRotationValue(float rotationToClamp)
	{
		float clampedRotation = rotationToClamp;
		if (clampedRotation > 180.0f)
		{
			clampedRotation -= 360.0f;
		}
		return clampedRotation;
	}


	//If moving on X, replace Z to zero and vice versa
	[ClientRpc]
	public void ResetAngleCommand(float angleToCheck, bool isX)
	{
		angleToCheck = GetAccurateRotationValue(angleToCheck);

		if (!isX)
		{
			if (angleToCheck > 0)
			{
				m_arena.transform.Rotate(Vector3.right, -m_movementSpeed * Time.deltaTime);
			}
			else if (angleToCheck < 0)
			{
				m_arena.transform.Rotate(Vector3.right, m_movementSpeed * Time.deltaTime);
			}
		}
		else
		{
			if (angleToCheck > 0)
			{
				m_arena.transform.Rotate(Vector3.forward, -m_movementSpeed * Time.deltaTime);
			}
			else if (angleToCheck < 0)
			{
				m_arena.transform.Rotate(Vector3.forward, m_movementSpeed * Time.deltaTime);
			}
		}

		ReplaceRcp(m_arena.transform.localEulerAngles);
	}

	[ClientRpc]
	public void ReplaceRcp(Vector3 newAngle)
	{
		m_arena.transform.localEulerAngles = newAngle;
	}
}
