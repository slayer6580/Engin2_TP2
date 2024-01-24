using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
     [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
     public Rigidbody RB { get; private set; }
     public Animator Animator { get; set; }
     private CapsuleCollider PlayerCollider { get; set; }

    [SerializeField] private CharacterFloorTrigger m_floorTrigger;

    [field: Header("Movement")]
    [field: SerializeField] public float AccelerationValue { get; private set; }
    [field: SerializeField] public float MaxVelocity { get; private set; }
    [field: SerializeField][field: Range(0.0f, 1.0f)] public float AccelerationSideMultiplier { get; private set; }
    [field: SerializeField][field: Range(0.0f, 1.0f)] public float AccelerationBackMultiplier { get; private set; }
    [field: SerializeField][field: Range(1.0f, 5.0f)] public float DragOnGround { get; private set; }

    [field: Header("Jumping")]
    [field: SerializeField] public float JumpIntensity { get; private set; } = 1000.0f;
    [field: SerializeField] public float AccelerationAirMultiplier { get; private set; }
    [field: Range(0.0f, 5.0f)][field: SerializeField] public float DragOnAir { get; private set; }
    public float SideSpeed { get; private set; }
    public float BackSpeed { get; private set; }


    [SerializeField] private PhysicMaterial m_defaultPhysicMaterial;
    [SerializeField] private PhysicMaterial m_inAirPhysicMaterial;

    List<CharacterState> m_possibleStates;
    CharacterState m_currentState;

    [HideInInspector] public bool m_InAir = false;
    [HideInInspector] public bool m_CanJump = true;

    private void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new InAirState());
        m_possibleStates.Add(new JumpState());

        SideSpeed = AccelerationValue * AccelerationSideMultiplier;
        BackSpeed = AccelerationValue * AccelerationBackMultiplier;
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();
    }


    private void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    private void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    public bool IsInContactWithFloor()
    {
       return m_floorTrigger.IsOnFloor;
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


    public void UpdateAnimationValue(Vector3 movementVecValue)
    {
        Animator.SetFloat("MoveX", movementVecValue.x);
        Animator.SetFloat("MoveY", movementVecValue.y);
    }

    public void InAirPhysic()
    {
        PlayerCollider.material = m_inAirPhysicMaterial;
    }

    public void DefaultPhysic()
    {
        PlayerCollider.material = m_defaultPhysicMaterial;

    }

}



