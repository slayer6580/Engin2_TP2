using UnityEngine;

public class FreeState : CharacterState
{
    Vector3 m_lastVectorDirection = Vector3.zero;

    private const float LANDING_DURATION = 0.7f;
    private float m_currentTimer = 0;
    public override void OnEnter()
    {
        m_stateMachine.RB.drag = m_stateMachine.DragOnGround;
    }

    public override void OnUpdate()
    {
        JumpDelay();
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

    private void JumpDelay()
    {
        if (!m_stateMachine.m_CanJump)
        {
            m_currentTimer += Time.deltaTime;
            if (m_currentTimer > LANDING_DURATION)
            {
                m_stateMachine.m_CanJump = true;
            }
        }
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
            totalSpeed += m_stateMachine.AccelerationValue;
            m_lastVectorDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.SideSpeed;
            m_lastVectorDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVector += Vector3.ProjectOnPlane(-m_stateMachine.Camera.transform.forward, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.BackSpeed;
            m_lastVectorDirection += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
            inputsNumber++;
            totalSpeed += m_stateMachine.SideSpeed;
            m_lastVectorDirection += Vector3.right;
        }
        if (m_lastVectorDirection != Vector3.zero)
        {
            m_lastVectorDirection.Normalize();
        }

        float normalizeSpeed = 0;
        Vector3 normalizedVector = Vector3.zero;

        if (inputsNumber != 0)
        {
            normalizeSpeed = totalSpeed / inputsNumber;
            normalizedVector = totalVector.normalized;
        }

        m_lastVectorDirection = m_lastVectorDirection * (m_stateMachine.RB.velocity.magnitude / m_stateMachine.MaxVelocity);
        m_stateMachine.RB.AddForce(normalizedVector * normalizeSpeed, ForceMode.Acceleration);
    }

    private void SetMaxVelocity()
    {
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxVelocity;
        }
    }
}
