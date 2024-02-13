using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
	// Pour les ma�tres du jeu
	void OnPlayerClicked(GameMasterController player); // seuls les ma�tres du jeu peuvent clicker
	void OnPlayerClickUp(GameMasterController player);

}