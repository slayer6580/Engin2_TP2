using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BumpedState : CharacterState
{
	private float m_bumpedTime = 0.5f;
	private float m_bumbedTimer = 0;
	private float m_maxPushVelocity = 20;

	public override void OnEnter()
	{
		m_stateMachine.m_JumpLeft = 1;
		m_bumbedTimer = m_bumpedTime;
		m_stateMachine.m_InAir = true;
		m_stateMachine.RB.drag = m_stateMachine.DragOnAir;
		m_stateMachine.InAirPhysic();

		Debug.Log("BumpedState STATE");
	}

	public override void OnExit()
	{
		
		Debug.Log("Exit state: InAirState\n");
		m_stateMachine.GroundPhysic();
	}

	public override void OnUpdate()
	{
		m_bumbedTimer -= Time.deltaTime;
		SetMaxVelocityInAir();
		m_stateMachine.BeingBumped(false);
	}

	public override void OnFixedUpdate()
	{

	}

	
	public override bool CanEnter(IState currentState)
	{
		//This must be run in Update absolutely
		if (m_stateMachine.HasJustBeenBumped())
		{
			Debug.Log("JUST ENTER: " + m_stateMachine.HasJustBeenBumped());
			return true;
		}

		return false;
	}

	public override bool CanExit()
	{

		return m_bumbedTimer <= 0;
	}

	// pour avoir une vitesse dans les airàrs maximal
	private void SetMaxVelocityInAir()
	{
		if (m_stateMachine.RB.velocity.magnitude > m_maxPushVelocity)
		{
			m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_maxPushVelocity;
		}
	}

	private void ApplyGravityPull()
	{
		Vector3 gravity = new Vector3(0, m_stateMachine.GravityForce, 0);
		m_stateMachine.RB.AddForce(gravity, ForceMode.Acceleration);
	}

}

