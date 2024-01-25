using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomPlayer : NetworkBehaviour
{
    private bool m_isGameMaster;                //Which side the player has chosen
    [SyncVar]public int m_slotSelected = -1;    //The position in the lobby

    public void SetIfGameMaster(bool isGameMaster)
    {
		m_isGameMaster = isGameMaster;
	}


}
