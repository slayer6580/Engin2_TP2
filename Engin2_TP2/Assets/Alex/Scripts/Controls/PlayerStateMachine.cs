using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [field: Header("Character Model")]
    [field: SerializeField] public GameObject Model { get; private set; }

    [field: Header("Camera")]
    [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    [field: SerializeField] public Vector3 Direction { get; private set; }

    [field: Header("Movement")]
    [field: SerializeField] public float GroundSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
    [field: Range(1.0f, 3.0f)][field: SerializeField] public float SpeedMultiplier { get; private set; }
    [field: SerializeField] public float AccelerationRate { get; private set; }
    [field: SerializeField] public float MaxVelocityOnGround { get; private set; }
    [field: SerializeField][field: Range(1.0f, 5.0f)] public float DragOnGround { get; private set; }

    [field: Header("Jumping")]
    [field: SerializeField] public float JumpIntensity { get; private set; } = 1000.0f;
    [field: SerializeField] public int MaxJump { get; private set; }
    [field: Range(0.0f, 10.0f)] [field: SerializeField] public float AirMoveSpeed_Multiplier { get; private set; }
    [field: SerializeField] public float MaxVelocityInAir { get; private set; }
    [field: Range(0.0f, 5.0f)] [field: SerializeField] public float DragOnAir { get; private set; }
    [field: Range(-25.0f, 0f)][field: SerializeField] public float GravityForce { get; private set; }
    [field: SerializeField] public float LandingDelay { get; private set; }
    public Rigidbody RB { get; private set; }
    public Animator Animator { get; set; }
    private CapsuleCollider PlayerCollider { get; set; }

    [field: Header("Stamina")]
    public StaminaPlayer StaminaPlayer { get; set; }



    [SerializeField] private CharacterFloorTrigger m_floorTrigger;
    [SerializeField] private PhysicMaterial m_groundPhysicMaterial;
    [SerializeField] private PhysicMaterial m_inAirPhysicMaterial;

    List<CharacterState> m_possibleStates;
    CharacterState m_currentState;

    [HideInInspector] public bool m_InAir = false;
    [HideInInspector] public bool m_CanJump = true;
    [HideInInspector] public int m_JumpLeft;

	private bool m_JustBeenBumped = false;

    public void BeingBumped(bool value)
    {
        print("Bump: " + value);
        m_JustBeenBumped = value;
	}

	// regarder si un objet bumper nous a toucher
	public bool HasJustBeenBumped()
	{
		return m_JustBeenBumped;
	}


	private void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
		m_possibleStates.Add(new BumpedState());
		m_possibleStates.Add(new InAirState());
        m_possibleStates.Add(new JumpState());
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
        StaminaPlayer = GetComponent<StaminaPlayer>();
    }

    void Start()
    {
        CreatePossibleStates();
   
        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

    }

    private void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
        UpdateAnimatorInAirBool();
    }


    private void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    // regarder si notre object floor trigger touche le sol
    public bool IsInContactWithFloor()
    {
       return m_floorTrigger.IsOnFloor;
    }
	
	
	private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState.Equals(state))
            {
                continue;
            }

            if (state.CanEnter(m_currentState))
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
    }

    public CharacterState GetCurrentState()
    {
        return m_currentState;
    }


    // Pour appeller le physic matérial dans les airs
    public void InAirPhysic()
    {
        PlayerCollider.material = m_inAirPhysicMaterial;
    }

    // Pour appeller le physic matérial sur le sol
    public void GroundPhysic()
    {
        PlayerCollider.material = m_groundPhysicMaterial;
    }

    public void SetGroundSpeed(float newSpeed)
    {
        GroundSpeed = newSpeed;
    }

    public void SetSprintMultiplier(float newSpeed)
    {
        SpeedMultiplier = newSpeed;
    }

    public void SetJumpForce(float newIntensity)
    {
        JumpIntensity = newIntensity;
    }
    public void SetOrientation(Vector3 direction)
    {
        Model.transform.LookAt(direction + transform.position);
    }
    private void UpdateAnimatorInAirBool()
    {
        Animator.SetBool("InAir", !m_floorTrigger.IsOnFloor);
    }

}



