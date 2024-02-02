using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
     * Note: Le script va repousser tout ce qui a un PlayerStateMachine et un rigidbody
     * Attention: l'objet sur lequel ce script est mis doit avoir un rigidbody!!
*/
public class Bumper : MonoBehaviour, IInteractable
{
    
    [SerializeField] private float pushForce = 1000f; // Adjustable force of the push
    [SerializeField] private float bumperForceReduction = 2.0f; // le bumper bouge moins que le player sur impact
    [SerializeField] private bool bumperObjectMovesOnImpact = true;
    [SerializeField] private bool canBeMovedByGameMaster = false;

    private Rigidbody bumperRigidbody;
    public ParticleSystem touchEffect;
    public void Awake()
    {
         bumperRigidbody = this.GetComponent<Rigidbody>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStateMachine stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
        if (stateMachine == null || bumperRigidbody==null)
            return;

        ContactPoint contactPoint = collision.contacts[0];
        Vector3 oppositeDirection = contactPoint.normal;

        stateMachine.RB.AddForce(-oppositeDirection * pushForce, ForceMode.Impulse);
        PlayParticleEffect();
        if (bumperObjectMovesOnImpact)
        { 
            bumperRigidbody.AddForce(oppositeDirection * pushForce / bumperForceReduction, ForceMode.Impulse); 
        }
    }


    // Code de l'interface. Pour les bumpers controllés par les Game Masters:
    public void OnPlayerClicked(Player player)
    { 
        // ne fait rien, mais éventuellement, les maîtres du jeu pourraient déplacer le bumper   
        // if (player.type == maitre)
        // ...

        // Pour le moment, on mets le script sur un objet; c'est un autre script sur l'objet qui permet le déplacement.
        //IInteractable
    }
    public void OnPlayerCollision(Player player)
    {
        // On passe par la state machine finalement
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