using UnityEngine;

public class InAirState : CharacterState
{

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
            totalSpeed += m_stateMachine.GroundSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.GroundSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.forward, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.GroundSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.GroundSpeed;
        }

        float finalSpeed = 0;
        Vector3 normalizedVector = Vector3.zero;

        if (inputsNumber != 0)
        {
            finalSpeed = (totalSpeed / inputsNumber) * m_stateMachine.AirMoveSpeed_Multiplier;
            normalizedVector = totalVector.normalized;
        }

        m_stateMachine.RB.AddForce(normalizedVector * finalSpeed, ForceMode.Acceleration);

       
    }

    public override bool CanEnter(IState currentState)
    {
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

        return false;

    }

    // pour avoir une vitesse dans les air�rs maximal
    private void SetMaxVelocityInAir()
    {
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocityOnGround)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_stateMachine.MaxVelocityInAir;
        }
    }

    private void ApplyGravityPull() 
    {
        Vector3 gravity = new Vector3(0, m_stateMachine.GravityForce, 0);
        m_stateMachine.RB.AddForce(gravity, ForceMode.Acceleration);
    }

}

