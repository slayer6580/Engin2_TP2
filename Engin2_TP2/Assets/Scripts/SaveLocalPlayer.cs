using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
	public class SaveLocalPlayer : NetworkBehaviour
	{
		[SyncVar] public bool m_isGameMaster;

	}
}



