using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
	// Pour les maîtres du jeu
	void OnPlayerClicked(GameMasterController player); // seuls les maîtres du jeu peuvent clicker
	void OnPlayerClickUp(GameMasterController player);

}