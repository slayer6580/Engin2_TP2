using UnityEngine;

public class JumpState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.5f;
    private float m_currentStateTimer = 0.0f;

    public override void OnEnter()
    {
        m_stateMachine.m_InAir = true;
        m_stateMachine.RB.drag = m_stateMachine.DragOnAir;
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
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
        if ((!m_stateMachine.m_InAir && m_stateMachine.m_CanJump))
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        return false;
 


    }

    public override bool CanExit()
    {
        return true;
    }
}
