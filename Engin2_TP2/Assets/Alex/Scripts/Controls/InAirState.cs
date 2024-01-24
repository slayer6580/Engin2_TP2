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
        m_stateMachine.DefaultPhysic();
    }

    public override void OnFixedUpdate()
    {
        InAirMovement();
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
            totalSpeed += m_stateMachine.AccelerationValue;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.SideSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.forward, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.BackSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.SideSpeed;
        }

        float finalSpeed = 0;
        Vector3 normalizedVector = Vector3.zero;

        if (inputsNumber != 0)
        {
            finalSpeed = (totalSpeed / inputsNumber) * m_stateMachine.AccelerationAirMultiplier;
            normalizedVector = totalVector.normalized;
        }

        m_stateMachine.RB.AddForce(normalizedVector * finalSpeed, ForceMode.Acceleration);

        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxVelocity;
        }
    }

    public override void OnUpdate()
    {

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
            m_stateMachine.m_CanJump = false;                                                   
            m_stateMachine.m_InAir = false;  // IMPORTANT //
            return true;
        }

        return false;

    }


}

