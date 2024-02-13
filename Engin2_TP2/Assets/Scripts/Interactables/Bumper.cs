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
    
    [SerializeField] private float m_pushForce = 1000f; // Adjustable force of the push
    [SerializeField] private float m_bumperForceReduction = 2.0f; // le bumper bouge moins que le player sur impact
    [SerializeField] private float m_addedUpForce = 0f; 
    [SerializeField] private bool m_bumperObjectMovesOnImpact = true;


    private Rigidbody m_bumperRigidbody;
    public ParticleSystem m_touchEffect;
    public void Awake()
    {
		m_bumperRigidbody = this.GetComponent<Rigidbody>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStateMachine stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
        if (stateMachine != null)
        {
			AudioManager.GetInstance().CmdPlaySoundEffectsOneShotAll(ESound.bumper, gameObject.transform.position);
			stateMachine.BeingBumped(true);
		}
       
        

		if (stateMachine == null || m_bumperRigidbody == null)
            return;

        ContactPoint contactPoint = collision.contacts[0];
        Vector3 oppositeDirection = contactPoint.normal;

     
		stateMachine.RB.AddForce(-oppositeDirection * m_pushForce, ForceMode.Impulse);
		stateMachine.RB.AddForce(new Vector3(0, m_addedUpForce, 0) * m_pushForce, ForceMode.Impulse);

		PlayParticleEffect();
        if (m_bumperObjectMovesOnImpact)
        {
			m_bumperRigidbody.AddForce(oppositeDirection * m_pushForce / m_bumperForceReduction, ForceMode.Impulse); 
        }
    }

    
    void PlayParticleEffect()
    {
        if (m_touchEffect != null)
        {
			m_touchEffect.Play();
        }
    }


}