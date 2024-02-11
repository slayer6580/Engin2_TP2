using UnityEngine;
using static AudioManager;
using UnityEngine.UIElements;
using System;
using System.Security.Principal;
using Mirror;

public class JumpState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.5f;
    private float m_currentStateTimer = 0.0f;
	private bool m_isSprinting = false; //Added
	private float m_maxSpeed;//Added
	private bool m_jumpFromGround;

	private float frames = 0;
	public override void OnEnter()
    {
        if (m_stateMachine.IsInContactWithFloor())
        {
			m_jumpFromGround = true;
        }

        m_stateMachine.m_InAir = true;
        m_stateMachine.RB.drag = m_stateMachine.DragOnAir;
        m_stateMachine.Animator.SetBool("Jump", true);
		//Reset Y velocity 
		m_stateMachine.RB.velocity = new Vector3(m_stateMachine.RB.velocity.x, 0, m_stateMachine.RB.velocity.z);

		//Play sound
		AudioManager.GetInstance().CmdPlaySoundEffectsOneShotAll(ESound.jump, m_stateMachine.transform.position);

        //Check if its the first jump, the first jump is free
        if (m_stateMachine.m_JumpLeft != m_stateMachine.MaxJump) 
        {
            m_stateMachine.StaminaPlayer.JumpCost();
        }
        m_stateMachine.m_JumpLeft--;
        Debug.LogWarning("Jump Left: " + m_stateMachine.m_JumpLeft);

		// Force du saut

		if (m_jumpFromGround == true) 
		{
            if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false)
            {
                m_stateMachine.RB.velocity = Vector3.zero;
            }
        }
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        // pour le la durée du state jump

        m_currentStateTimer = STATE_EXIT_TIMER;

        Debug.Log("Enter state: JumpState\n");
		frames = 0;
    }

    public override void OnExit()
    {
        m_currentStateTimer = 0;
        Debug.LogWarning("Frames: " + frames);
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {
		InAirMovement();
	}

	//Added
	private void InAirMovement()
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
		if (m_isSprinting == true && m_stateMachine.StaminaPlayer.CanUseStamina())
		{
			m_stateMachine.StaminaPlayer.RunCost();
			totalSpeed *= m_stateMachine.SpeedMultiplier;
		}

		// Pour mélanger équalement les vitesses de toute les directions appuyés (exemple: haut et gauche)
		if (inputsNumber != 0)
		{
			normalizeSpeed = totalSpeed / inputsNumber;
			normalizedVector = totalVector.normalized;
			
		}

		// Bouger
		m_stateMachine.RB.AddForce(normalizedVector * normalizeSpeed, ForceMode.Acceleration);

	}

	public override void OnUpdate()
    {
		SetMaxVelocityInAir();
		m_currentStateTimer -= Time.deltaTime;
		Sprint();
		frames++;
	}

	// pour avoir une vitesse dans les airàrs maximal
	private void SetMaxVelocityInAir()
	{
		if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocityOnGround)
		{
			Vector3 velocity = m_stateMachine.RB.velocity;
			Vector2 newVelocityXZ = new Vector2(velocity.x, velocity.z).normalized * m_stateMachine.MaxVelocityInAir;
			Vector3 newVelocity = new Vector3(newVelocityXZ.x, velocity.y, newVelocityXZ.y);
			m_stateMachine.RB.velocity = newVelocity;

			//m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_stateMachine.MaxVelocityInAir;
		}
	}

	public override bool CanEnter(IState currentState)
    {
		if (m_stateMachine.HasJustBeenBumped())
		{
			return false;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			//This must be run in Update absolutely
			if (m_stateMachine.m_JumpLeft > 1)
			{
				return Input.GetKeyDown(KeyCode.Space);
			}

			if (m_stateMachine.m_JumpLeft == 1)
			{
				if (m_stateMachine.StaminaPlayer.CanUseStamina())
				{
					return true;
				}
				else
				{
					AudioManager.GetInstance().CmdPlaySoundEffectsOneShotTarget(ESound.noStamina, m_stateMachine.transform.position, NetworkClient.localPlayer.gameObject.GetComponent<NetworkIdentity>());
				}
			}
		}

			return false;
    }

    public override bool CanExit()
    {
		if (m_stateMachine.HasJustBeenBumped())
		{
			return true;
		}
		return m_currentStateTimer <= 0;
    }

	//Added
	private void Sprint()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			m_maxSpeed = m_stateMachine.SprintSpeed;
			m_isSprinting = true;
			return;
		}
		m_maxSpeed = m_stateMachine.GroundSpeed;
		m_isSprinting = false;
	}
}
