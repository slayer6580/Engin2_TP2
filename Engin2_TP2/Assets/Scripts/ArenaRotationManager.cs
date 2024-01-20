using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRotationManager : NetworkBehaviour
{
	[SerializeField] private List<ArenaRotation> m_rotators = new List<ArenaRotation>();

	[Command(requiresAuthority = false)]
	public void ManageRotatorsCommand(bool _value)
	{
		ManageRotators(_value);
	}

	[ClientRpc]
	public void ManageRotators(bool _value)
	{
		foreach (ArenaRotation rotator in m_rotators)
		{
			rotator.ManageRotator(_value);
		}
	}

}
