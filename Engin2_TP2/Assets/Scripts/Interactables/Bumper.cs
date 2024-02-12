using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;
/* 
     * Note: Le script va repousser tout ce qui a un PlayerStateMachine et un rigidbody
     * Attention: l'objet sur lequel ce script est mis doit avoir un rigidbody!!
*/
public class Bumper : MonoBehaviour
{
    
    [SerializeField] private float pushForce = 1000f; // Adjustable force of the push
    [SerializeField] private float bumperForceReduction = 2.0f; // le bumper bouge moins que le player sur impact
    [SerializeField] private float addedUpForce = 0f; 
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
        if (stateMachine != null)
        {
			AudioManager.GetInstance().CmdPlaySoundEffectsOneShotAll(ESound.bumper, gameObject.transform.position);
			stateMachine.BeingBumped(true);
		}
       
        

		if (stateMachine == null || bumperRigidbody==null)
            return;

        ContactPoint contactPoint = collision.contacts[0];
        Vector3 oppositeDirection = contactPoint.normal;

     
		stateMachine.RB.AddForce(-oppositeDirection * pushForce, ForceMode.Impulse);
		stateMachine.RB.AddForce(new Vector3(0, addedUpForce, 0) * pushForce, ForceMode.Impulse);

		PlayParticleEffect();
        if (bumperObjectMovesOnImpact)
        { 
            bumperRigidbody.AddForce(oppositeDirection * pushForce / bumperForceReduction, ForceMode.Impulse); 
        }
    }

    
    void PlayParticleEffect()
    {
        if (touchEffect != null)
        {
            touchEffect.Play();
        }
    }


}