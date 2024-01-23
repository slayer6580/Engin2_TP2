using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
   
    // Certaines obstacles ont un effet sur le player, si ce n'est pas le cas, �a ne fera rien

    void OnPlayerCollision(Player player);


    // Pour les ma�tres du jeu
    void OnPlayerClicked(GameMasterController player); // seuls les ma�tres du jeu peuvent clicker
	void OnPlayerClickUp(GameMasterController player);

	// Ce qui se passe quand on clic � l'objet (bouge, tourne, particules...)
	void UpdateInteractableObject(GameMasterController player);


	// N�cessaire?
	// void OnPlayerNearby(Player player);

}