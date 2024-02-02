using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour, IInteractable
{
    /* 
     * Note: Le script va repousser tout ce qui 1) peut causer des collisions 2) a le script player attach�
     * On pourrait plut�t cr�er un script "Bumpable" qui d�terminerait quel object est repouss� par le bumper
     */
    [SerializeField] private float pushForce = 10f; // Adjustable force of the push
    [SerializeField] private float bumperForceReduction = 2.0f; // le bumper bouge moins que le player sur impact
    [SerializeField] private bool bumperObjectMovesOnImpact = true;
    [SerializeField] private bool canBeMovedByGameMaster = false;

    private Rigidbody bumperRigidbody;
    private BoxCollider bumperCollider; // jai essay� un mesh collider, trop lent
    

    public ParticleSystem touchEffect;
    public void Awake()
    {
         bumperRigidbody = this.GetComponent<Rigidbody>(); 
         bumperCollider = this.GetComponent<BoxCollider>();
    }

    public void OnPlayerCollision(PlayerStateMachine playerStateMachine)
    {
        PushPlayer(playerStateMachine);
        PlayParticleEffect();
        UpdateInteractableObject(playerStateMachine); // idealement ne passer que le transform du joueur ici, ca bouge le bumper
    }

    void PushPlayer(PlayerStateMachine playerStateMachine)
    {
        Rigidbody playerRigidbody = playerStateMachine.gameObject.GetComponent<Rigidbody>();

        //if (player.GetType() == player)
        //    return;
        // Comme les ma�tres n'ont pas de Rigidbody, le check ci-bas suffit

        if (playerRigidbody != null)
        {
            Vector3 bumpForce = CalculateBumpForce(playerStateMachine);

            // Apply force to the player's rigidbody
            playerRigidbody.AddForce(bumpForce, ForceMode.Impulse);
        }
    } // NOTE: Techniquement les ma�tres du jeu ne peuvent pas bumper dans les bumpers, mais il faudrait impl�menter un safecheck...

   
     void OnCollisionEnter(Collision collision)
    {
        PlayerStateMachine playerStateMachine = collision.collider.GetComponent<PlayerStateMachine>(); // le player doit avoir un script nomm� PlayerStateMachine sur lui
        if (playerStateMachine != null)
        {
            OnPlayerCollision(playerStateMachine); // ou encore, appeler IBumpable sur le joueur? A-t-on vraiment besoin d'une sous-fonction ici? Le bumper ne bump que les players
        }
    }

    public void OnPlayerClicked(Player player)
    { 
        // ne fait rien, mais �ventuellement, les ma�tres du jeu pourraient d�placer le bumper   
        // if (player.type == maitre)
        // ...

        // Pour le moment, on mets le script sur un objet; c'est un autre script sur l'objet qui permet le d�placement.
        //IInteractable
    }
    public void OnPlayerCollision(Player player)
    {
        // On passe par la state machine finalement
    }

    public void UpdateInteractableObject(PlayerStateMachine playerStateMachine)
    { 
       
        if (bumperRigidbody != null && bumperObjectMovesOnImpact)
        {
            Vector3 bumpForce = CalculateBumpForce(playerStateMachine);
            bumperRigidbody.AddForce(-bumpForce/bumperForceReduction, ForceMode.Impulse);
        }

    }

    Vector3 CalculateBumpForce(PlayerStateMachine playerStateMachine)
    {
  
        Vector3 pushDirection = this.transform.position- playerStateMachine.gameObject.transform.position;
        pushDirection.y = 0;            // d�placement sur le planXZ seulement
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