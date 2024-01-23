using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
   
    // Certaines obstacles ont un effet sur le player, si ce n'est pas le cas, ça ne fera rien

    void OnPlayerCollision(Player player);


    // Pour les maîtres du jeu
    void OnPlayerClicked(Player player); // seuls les maîtres du jeu peuvent clicker


    // Ce qui se passe quand on clic à l'objet (bouge, tourne, particules...)
    void UpdateInteractableObject(Player player);


    // Nécessaire?
    // void OnPlayerNearby(Player player);



}