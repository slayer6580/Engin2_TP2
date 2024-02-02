using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour, IInteractable
{
    /* 
     * Note: Le script va repousser tout ce qui 1) peut causer des collisions 2) a le script player attaché
     * On pourrait plutôt créer un script "Bumpable" qui déterminerait quel object est repoussé par le bumper
     */
    [SerializeField] private float pushForce = 10f; // Adjustable force of the push
    [SerializeField] private float bumperForceReduction = 2.0f; // le bumper bouge moins que le player sur impact
    [SerializeField] private bool bumperObjectMovesOnImpact = true;
    [SerializeField] private bool canBeMovedByGameMaster = false;

    private Rigidbody bumperRigidbody;
    private BoxCollider bumperCollider; // jai essayé un mesh collider, trop lent
    

    public ParticleSystem touchEffect;
    public void Awake()
    {
         bumperRigidbody = this.GetComponent<Rigidbody>(); 
         bumperCollider = this.GetComponent<BoxCollider>();
    }

    public void OnPlayerCollision(Player player)
    {
        PushPlayer(player);
        PlayParticleEffect();
        UpdateInteractableObject(player); // idealement ne passer que le transform du joueur ici, ca bouge le bumper
    }

    void PushPlayer(Player player)
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();

        //if (player.GetType() == player)
        //    return;
        // Comme les maîtres n'ont pas de Rigidbody, le check ci-bas suffit

        if (playerRigidbody != null)
        {
            Vector3 bumpForce = CalculateBumpForce(player);

            // Apply force to the player's rigidbody
            playerRigidbody.AddForce(bumpForce, ForceMode.Impulse);
        }
    } // NOTE: Techniquement les maîtres du jeu ne peuvent pas bumper dans les bumpers, mais il faudrait implémenter un safecheck...

   
     void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>(); // le player doit avoir un script nommé Player sur lui
        if (player != null)
        {
            OnPlayerCollision(player); // ou encore, appeler IBumpable sur le joueur? A-t-on vraiment besoin d'une sous-fonction ici? Le bumper ne bump que les players
        }
    }

    public void OnPlayerClicked(Player player)
    { 
        // ne fait rien, mais éventuellement, les maîtres du jeu pourraient déplacer le bumper   
        // if (player.type == maitre)
        // ...

        // Pour le moment, on mets le script sur un objet; c'est un autre script sur l'objet qui permet le déplacement.
        //IInteractable
    }

    public void UpdateInteractableObject(Player player)
    { 
       
        if (bumperRigidbody != null && bumperObjectMovesOnImpact)
        {
            Vector3 bumpForce = CalculateBumpForce(player);
            bumperRigidbody.AddForce(-bumpForce/bumperForceReduction, ForceMode.Impulse);
        }

    }

    Vector3 CalculateBumpForce(Player player)
    {
  
        Vector3 pushDirection = this.transform.position-player.transform.position;
        pushDirection.y = 0;            // déplacement sur le planXZ seulement
        pushDirection.Normalize();      // merci soufiane !! 

        return pushDirection * pushForce / 10.0f;
    }

    void PlayParticleEffect()
    {
        if (touchEffect != null)
        {
            touchEffect.Play();
        }
    }

	public void OnPlayerClicked(GameMasterController player)
	{
		throw new System.NotImplementedException(); // drag and drop?
	}

	public void OnPlayerClickUp(GameMasterController player)
	{
		throw new System.NotImplementedException();
	}

	public void UpdateInteractableObject(GameMasterController player)
	{
		throw new System.NotImplementedException();
	}
}