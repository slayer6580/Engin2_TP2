using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [field: Header("Camera")]
    [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }

    [field: Header("Movement")]
    [field: SerializeField] public float GroundSpeed { get; private set; }
    [field: SerializeField] public float MaxVelocityOnGround { get; private set; }
    [field: SerializeField][field: Range(0.0f, 1.0f)] public float SideSpeed_Multiplier { get; private set; }
    [field: SerializeField][field: Range(0.0f, 1.0f)] public float BackSpeed_Multiplier { get; private set; }
    [field: SerializeField][field: Range(1.0f, 5.0f)] public float DragOnGround { get; private set; }

    [field: Header("Jumping")]
    [field: SerializeField] public float JumpIntensity { get; private set; } = 1000.0f;
    [field: Range(0.0f, 1.0f)] [field: SerializeField] public float AirMoveSpeed_Multiplier { get; private set; }
    [field: SerializeField] public float MaxVelocityInAir { get; private set; }
    [field: Range(0.0f, 5.0f)] [field: SerializeField] public float DragOnAir { get; private set; }
    [field: SerializeField] public float LandingDelay { get; private set; }
    public Rigidbody RB { get; private set; }
    public Animator Animator { get; set; }
    private CapsuleCollider PlayerCollider { get; set; }
    public float SideSpeed { get; private set; }
    public float BackSpeed { get; private set; }

    [SerializeField] private CharacterFloorTrigger m_floorTrigger;
    [SerializeField] private PhysicMaterial m_groundPhysicMaterial;
    [SerializeField] private PhysicMaterial m_inAirPhysicMaterial;

    List<CharacterState> m_possibleStates;
    CharacterState m_currentState;

    [HideInInspector] public bool m_InAir = false;
    [HideInInspector] public bool m_CanJump = true;

    private void CreatePossibleStates()
    {
        // Code Review:
        // Rien à dire, sinon que c'est excellent!
        //
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new InAirState());
        m_possibleStates.Add(new JumpState());
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();

        // Code Review:
        // Ajouter des if (Animator != null) 
        // Si on a le temps, encore une fois
        //
    }

    void Start()
    {
        CreatePossibleStates();
        SetSideAndBackSpeed();
   
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
    }

    private void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    // Set la vitesse de coté et de reculont
    private void SetSideAndBackSpeed()
    {
        SideSpeed = GroundSpeed * SideSpeed_Multiplier;
        BackSpeed = GroundSpeed * BackSpeed_Multiplier;
    }

    // regarder si notre object floor trigger touche le sol
    public bool IsInContactWithFloor()
    {
        // Code Review:
        // Cela empêche le double saut. Attention aussi, comment détecter les collisions avec le sol?
        //
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

    public void SetJumpForce(float newIntensity)
    {
        JumpIntensity = newIntensity;
    }

}



