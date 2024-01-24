using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CharacterControllerStateMachine : BaseStateMachine<CharacterState>
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        CreatePossibleStates();
    }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        //m_possibleStates.Add(new FreeState());
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        TryStateTransition();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public bool IsInContactWithFloor()
    {
        //return m_floorTrigger.IsOnFloor;
        return false;
    }


}
