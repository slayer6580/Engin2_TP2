using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;
using UnityEngine.Events;
using Mirror;

public class ActivateObstacle : NetworkBehaviour, IInteractable
{

	[SerializeField] UnityEvent m_Deactivate;
	[SerializeField] UnityEvent<GameMasterController> m_Activate;

	
	public void OnPlayerClicked(GameMasterController player)
	{
		m_Activate.Invoke(player);
	}

	
	public void OnPlayerClickUp(GameMasterController player)
	{
		m_Deactivate.Invoke();
	}

	public void StaminaCost(GameMasterController player)
	{
		throw new System.NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
