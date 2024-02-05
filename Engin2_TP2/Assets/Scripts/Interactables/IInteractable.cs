using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Pour les maîtres du jeu
    void OnPlayerClicked(GameMasterController player); // seuls les maîtres du jeu peuvent clicker
	void OnPlayerClickUp(GameMasterController player);

	// Ce qui se passe quand on clic à l'objet (bouge, tourne, particules...)
	//void StaminaCost(GameMasterController player);


	// Nécessaire?
	// void OnPlayerNearby(Player player);

}