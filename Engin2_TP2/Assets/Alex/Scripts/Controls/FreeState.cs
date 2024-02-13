using Mirror;
using UnityEngine;
using static AudioManager;

public class FreeState : CharacterState
{
    Vector3 m_lastVectorDirection = Vector3.zero;
    private const float LANDING_DURATION = 0.7f;
    private float m_currentTimer = 0;
    private float m_maxSpeed;
    private bool m_isSprinting = false;
    private bool m_noMoreStaminaSfxHasPlay = false;

    private Vector3 m_velocity = Vector3.zero;
    public override void OnEnter()
    {
        m_stateMachine.RB.drag = m_stateMachine.DragOnGround;
        m_stateMachine.m_JumpLeft = m_stateMachine.MaxJump;
        Debug.Log("Enter state: FreeState\n");
    }

    public override void OnUpdate()
    {
        m_stateMachine.Animator.SetFloat("Speed", GetMovement());
        Sprint();
    }


    public override void OnFixedUpdate()
    {
        FreeStateMovement();
        SetMaxVelocity(); 
    }

    public override void OnExit()
    {
        m_currentTimer = 0;
    }

    public override bool CanEnter(IState currentState)
    {
		if (m_stateMachine.HasJustBeenBumped())
		{
			return false;
		}
		//Je ne peux entrer dans le FreeState que si je touche le sol
		if (m_stateMachine.IsInContactWithFloor())
        {
            return m_stateMachine.IsInContactWithFloor();
        }
        return false;

    }

    public override bool CanExit()
    {
        return true;
    }

    private void FreeStateMovement()
    {
        Vector3 totalVector = Vector3.zero;
        int inputsNumber = 0;
        float totalSpeed = 0;

        if (Input.GetKey(KeyCode.W))
        {
            totalVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.AccelerationRate;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.AccelerationRate;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.forward, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.AccelerationRate;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.AccelerationRate;
        }

        float normalizeSpeed = 0;
        Vector3 normalizedVector = Vector3.zero;

        // Si on sprint, applique le multiplier
        if (m_isSprinting == true)
        {
            m_stateMachine.StaminaPlayer.RunCost();
        }

        // Pour mélanger équalement les vitesses de toute les directions appuyés (exemple: haut et gauche)
        if (inputsNumber != 0)
        {
            normalizeSpeed = totalSpeed / inputsNumber;
            normalizedVector = totalVector.normalized;
            m_stateMachine.SetOrientation(normalizedVector);
        }

        // Bouger
        m_stateMachine.RB.AddForce(normalizedVector * normalizeSpeed, ForceMode.Acceleration);
    }

    // pour avoir une vitesse au sol maximal
    private void SetMaxVelocity()
    {
        if (m_stateMachine.RB.velocity.magnitude > m_maxSpeed)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_maxSpeed;
        }
    }

    private float GetMovement() 
    {
        return m_stateMachine.RB.velocity.magnitude / m_stateMachine.MaxVelocityOnGround;
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (m_stateMachine.StaminaPlayer.CanUseStamina())
            {
				m_maxSpeed = m_stateMachine.SprintSpeed * m_stateMachine.SpeedMultiplier;
				m_isSprinting = true;
				return;
			}
            else
            {
                if (m_noMoreStaminaSfxHasPlay == false)
                {
					AudioManager.GetInstance().CmdPlaySoundEffectsOneShotTarget(ESound.noStamina, m_stateMachine.transform.position, NetworkClient.localPlayer.gameObject.GetComponent<NetworkIdentity>());
                    m_noMoreStaminaSfxHasPlay = true;

				}
				return;
			}
           		
		}
        m_maxSpeed = m_stateMachine.GroundSpeed * m_stateMachine.SpeedMultiplier;
        m_isSprinting = false;
		m_noMoreStaminaSfxHasPlay = false;
	}
}
