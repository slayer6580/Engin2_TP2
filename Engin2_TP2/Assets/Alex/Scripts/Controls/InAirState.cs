using UnityEngine;

public class InAirState : CharacterState
{
	private bool m_isSprinting = false; //Added
	private float m_maxSpeed;//Added

	public override void OnEnter()
    {
        m_stateMachine.m_InAir = true;
        m_stateMachine.RB.drag = m_stateMachine.DragOnAir;
        m_stateMachine.InAirPhysic();
        Debug.Log("Enter state: InAirState\n");
     
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: InAirState\n");
        m_stateMachine.GroundPhysic();
    }

    public override void OnUpdate()
    {
        SetMaxVelocityInAir();
		Sprint();
	}

    public override void OnFixedUpdate()
    {
        InAirMovement();
        ApplyGravityPull();
    }

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
			totalSpeed *= m_stateMachine.SprintSpeedMultiplier;
		}

		// Pour m�langer �qualement les vitesses de toute les directions appuy�s (exemple: haut et gauche)
		if (inputsNumber != 0)
		{
			normalizeSpeed = totalSpeed / inputsNumber;
			normalizedVector = totalVector.normalized;
			
		}

		// Bouger
		m_stateMachine.RB.AddForce(normalizedVector * normalizeSpeed, ForceMode.Acceleration);


	}

    public override bool CanEnter(IState currentState)
    {
		if (m_stateMachine.HasJustBeenBumped())
		{
			return false;
		}
		//This must be run in Update absolutely
		if (!m_stateMachine.IsInContactWithFloor())
        {
            return true;
        }

        return false;
    }

    public override bool CanExit()
    {

        if (m_stateMachine.IsInContactWithFloor())  
        {                           
            //m_stateMachine.m_CanJump = false;                                                   
            m_stateMachine.m_InAir = false;  // IMPORTANT //
            return true;
        }

        //double jump
        if (!m_stateMachine.IsInContactWithFloor() && m_stateMachine.m_JumpLeft > 0) 
        {
            return true;
        }

        if(m_stateMachine.HasJustBeenBumped())
        {
			return true;
		}

        return false;

    }

    // pour avoir une vitesse dans les air�rs maximal
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

    private void ApplyGravityPull() 
    {
        Vector3 gravity = new Vector3(0, m_stateMachine.GravityForce, 0);
        m_stateMachine.RB.AddForce(gravity, ForceMode.Acceleration);
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

