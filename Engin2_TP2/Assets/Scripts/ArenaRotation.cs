using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class ArenaRotation : NetworkBehaviour, IInteractable
{
	enum Point { North, East, South, West };
	[SerializeField] private ArenaRotationManager m_rotationManager; 
	[SerializeField] GameObject m_arena;
    [SerializeField] Point m_rotationPoint;
	[SerializeField] private float m_maxX;
	[SerializeField] private float m_minX;
	private Vector3 m_currentRotation = Vector3.zero;
	[field: SerializeField] public bool IsActivated { get; set; } = true;
	private bool m_isSelected;
	public bool IsSelected
	{
		get
		{
			return m_isSelected;
		}
		set
		{
			m_isSelected = value;
			if (value)
			{
				m_rotationManager.ManageRotatorsCommand(false);
			}
			else
			{
				m_rotationManager.ManageRotatorsCommand(true);
			}
		}
	}

	public void ManageRotator(bool _value)
	{
		IsActivated = _value;
	}

	void Update()
    {
		if (IsSelected)
		{
			switch (m_rotationPoint)
			{
				case Point.North:
					MoveArena(-Input.GetAxis("Mouse Y"), Mathf.Round(m_arena.transform.eulerAngles.x), new Vector3(1, 0, 0), true);
					break;

				case Point.South:
					MoveArena(Input.GetAxis("Mouse Y"), Mathf.Round(m_arena.transform.eulerAngles.x), new Vector3(1, 0, 0), true);
					break;

				case Point.East:
					MoveArena(Input.GetAxis("Mouse Y"), Mathf.Round(m_arena.transform.eulerAngles.z), new Vector3(0, 0, 1), false);
					break;

				case Point.West:
					MoveArena(-Input.GetAxis("Mouse Y"), Mathf.Round(m_arena.transform.eulerAngles.z), new Vector3(0, 0, 1), false);
					break;
			}
		}

		if (isServer)
		{
			ReplaceIfOverCommand();
		}	
	}

	[Command(requiresAuthority = false)]
	public void MoveArena(float mouseYAxis, float angleToCheck, Vector3 rotateBy, bool isX)
	{
		if (mouseYAxis > 0)
		{
			if (angleToCheck < (45 + m_maxX))
			{
				ClientRotate(rotateBy);
			}

		}
		else if (mouseYAxis < 0)
		{
			if (angleToCheck > (45 + m_minX))
			{
				ClientRotate(-rotateBy);
			}			
		}
	
		if(mouseYAxis != 0)
		{
			if (isX)
			{
				ResetAngle_Rpc(Mathf.Round(m_arena.transform.eulerAngles.z), new Vector3(0, 0, 1));
			}
			else
			{
				ResetAngle_Rpc(Mathf.Round(m_arena.transform.eulerAngles.x), new Vector3(1, 0, 0));				
			}
		}
	}
	
	[ClientRpc]
	public void ClientRotate(Vector3 currentRotation)
	{
		m_arena.transform.eulerAngles += currentRotation;
	}

	[ClientRpc]
	public void ResetAngle_Rpc(float angleToCheck, Vector3 resetPosition)
	{
		if (angleToCheck > 45)
		{
			m_arena.transform.eulerAngles += new Vector3(-1 * resetPosition.x, 0, -1 * resetPosition.z);
		}

		if (angleToCheck < 45)
		{
			m_arena.transform.eulerAngles += new Vector3(1 * resetPosition.x, 0, 1 * resetPosition.z);
		}
	}

	[Command(requiresAuthority = false)]
	public void ReplaceIfOverCommand()
	{
		ReplaceIfOver();
	}

	[ClientRpc]
	public void ReplaceIfOver()
	{
		float xAngle = Mathf.Round(m_arena.transform.eulerAngles.x);
		if (xAngle > 45 + m_maxX)
		{
			m_arena.transform.eulerAngles = new Vector3(45 + m_maxX, 0, 45);
		}
		else if (xAngle < 45 + m_minX)
		{
			m_arena.transform.eulerAngles = new Vector3(45 + m_minX, 0, 45);
		}


		float zAngle = Mathf.Round(m_arena.transform.eulerAngles.z);
		if (zAngle > 45 + m_maxX)
		{
			m_arena.transform.eulerAngles = new Vector3(45, 0, 45 + m_maxX);
		}
		else if (zAngle < 45 + m_minX)
		{
			m_arena.transform.eulerAngles = new Vector3(45, 0, 45 + m_minX);
		}
	}


	public void OnPlayerCollision(Player player)
	{
		throw new System.NotImplementedException();
	}

	public void OnPlayerClicked(GameMasterController player)
	{
		if (IsActivated)
		{
			IsSelected = true;
		}

		
	}
	public void OnPlayerClickUp(GameMasterController player)
	{
		IsSelected = false;
	}

	public void UpdateInteractableObject(GameMasterController player)
	{
		throw new System.NotImplementedException();
	}


}
