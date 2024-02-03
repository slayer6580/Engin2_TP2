using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;
using UnityEngine.Events;
using Mirror;

public class ActivateObstacle : NetworkBehaviour, IInteractable
{
	[SerializeField] UnityEvent m_Activate;
	[SerializeField] UnityEvent m_Deactivate;

	
	public void OnPlayerClicked(GameMasterController player)
	{
		m_Activate.Invoke();
	}

	
	public void OnPlayerClickUp(GameMasterController player)
	{
		m_Deactivate.Invoke();
	}

	public void OnPlayerCollision(Player player)
	{
		throw new System.NotImplementedException();
	}

	public void UpdateInteractableObject(GameMasterController player)
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
