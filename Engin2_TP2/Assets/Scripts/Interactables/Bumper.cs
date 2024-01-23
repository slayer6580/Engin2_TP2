using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour, IInteractable
{
    [SerializeField] private float pushForce = 10f; // Adjustable force of the push
    private Rigidbody bumperRigidbody;
    public void Awake()
    {
         bumperRigidbody = this.GetComponent<Rigidbody>();
    }

    public void OnPlayerCollision(Player player)
    {
        PushPlayer(player);
    }

    void PushPlayer(Player player)
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            Vector3 pushDirection = player.transform.position - this.transform.position;
            pushDirection.y = 0;            // déplacement sur le planXZ seulement
            pushDirection.Normalize();      // merci soufiane 

            // Apply force to the player's rigidbody
            playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    } // NOTE: Techniquement les maîtres du jeu ne peuvent pas bumper dans les bumpers, mais il faudrait implémenter un safecheck...

   
     void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            OnPlayerCollision(player);
        }
    }

    public void OnPlayerClicked(Player player)
    { 
        // ne fait rien, mais éventuellement, les maîtres du jeu pourraient déplacer le bumper   
        // if (player.type == maitre)
        // ...

    }

    public void UpdateInteractableObject(Player player)
    {
        
       
        if (bumperRigidbody != null)
        {
            Vector3 pushDirection = -(player.transform.position - this.transform.position);
            pushDirection.y = 0;            // déplacement sur le planXZ seulement
            pushDirection.Normalize();      // merci soufiane 

            // Apply force to the player's rigidbody
            bumperRigidbody.AddForce(pushDirection * pushForce/10.0f, ForceMode.Impulse);
        }

    }
}