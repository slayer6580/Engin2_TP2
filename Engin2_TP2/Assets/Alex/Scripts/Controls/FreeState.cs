using UnityEngine;

public class FreeState : CharacterState
{
    Vector3 m_lastVectorDirection = Vector3.zero;
    private const float LANDING_DURATION = 0.7f;
    private float m_currentTimer = 0;
    private bool m_isSprinting = false;
    public override void OnEnter()
    {
        m_stateMachine.RB.drag = m_stateMachine.DragOnGround;
        m_stateMachine.m_JumpLeft = m_stateMachine.MaxJump;
        Debug.LogWarning("JumpLeft: " + m_stateMachine.m_JumpLeft);
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
        //Je ne peux entrer dans le FreeState que si je touche le sol
        if (m_stateMachine.IsInContactWithFloor())
        {
            Debug.LogWarning("JE TOUCHE LE SOL");
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
            totalSpeed += m_stateMachine.GroundSpeed;
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

        // Si on sprint, applique le multiplier
        if (m_isSprinting == true && m_stateMachine.StaminaPlayer.CanUseStamina())
        {
            m_stateMachine.StaminaPlayer.RunCost();
            totalSpeed *= m_stateMachine.SprintSpeedMultiplier;
        }

        // Pour mélanger équalement les vitesses de toute les directions appuyés (exemple: haut et gauche)
        if (inputsNumber != 0)
        {
            normalizeSpeed = totalSpeed / inputsNumber;
            normalizedVector = totalVector.normalized;
        }

       // était utile pour donner en parametres a vitesse de l'animation
        m_lastVectorDirection = m_lastVectorDirection * (m_stateMachine.RB.velocity.magnitude / m_stateMachine.MaxVelocityOnGround);

        // Bouger
        m_stateMachine.RB.AddForce(normalizedVector * normalizeSpeed, ForceMode.Acceleration);
    }

    // pour avoir une vitesse au sol maximal
    private void SetMaxVelocity()
    {
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocityOnGround)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_stateMachine.MaxVelocityOnGround;
        }
    }

    private float GetMovement() 
    {
        float velocity = 0;
        velocity += Mathf.Abs(m_stateMachine.RB.velocity.x);
        velocity += Mathf.Abs(m_stateMachine.RB.velocity.y);
        velocity += Mathf.Abs(m_stateMachine.RB.velocity.z);
        velocity = velocity / m_stateMachine.MaxVelocityOnGround;
        velocity *= 1.50f;
        //Debug.LogWarning("Velocity: " + velocity);

        return velocity;

    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_isSprinting = true;
            return;
        }
        m_isSprinting = false;
    }
}
