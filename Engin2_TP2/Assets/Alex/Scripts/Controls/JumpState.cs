using UnityEngine;

public class JumpState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.5f;
    private float m_currentStateTimer = 0.0f;

    public override void OnEnter()
    {
        m_stateMachine.m_InAir = true;
        m_stateMachine.RB.drag = m_stateMachine.DragOnAir;
        m_stateMachine.Animator.SetBool("Jump", true);

        //Check if its the first jump, the first jump is free
        if (m_stateMachine.m_JumpLeft != m_stateMachine.MaxJump) 
        {
            m_stateMachine.StaminaPlayer.JumpCost();
        }
        m_stateMachine.m_JumpLeft--;
        Debug.LogWarning("Jump Left: " + m_stateMachine.m_JumpLeft);

        // Force du saut
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        // pour le la durée du state jump

        m_currentStateTimer = STATE_EXIT_TIMER;

        Debug.Log("Enter state: JumpState\n");

    }

    public override void OnExit()
    {
        m_currentStateTimer = 0;
 
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        //This must be run in Update absolutely
        if (m_stateMachine.m_JumpLeft > 0)
        {
            return Input.GetKeyDown(KeyCode.Space) && m_stateMachine.StaminaPlayer.CanUseStamina();
        }

        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }
}
