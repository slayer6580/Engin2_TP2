using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

[RequireComponent(typeof(NetworkIdentity))]

public class ObjectOnRail : NetworkBehaviour
{
	[Header("Important")]
	[SerializeField] private ObstacleManager m_obstacleManager;
	[SerializeField] private GameObject m_objectToMove;

	[Header("Settings")]
	[SerializeField] private bool m_isAuto;
    [SerializeField] private bool m_isLooping;
	[SerializeField] private float m_autoSpeed;

	[Header("Movement")]
	[SerializeField] private List<Transform> m_railPoints = new List<Transform>();
    [SerializeField] private List<int> m_railPointsPercentage = new List<int>();

    [SyncVar][SerializeField][Range (0,100)] private float m_positionOnRailPercent;
  
    private Vector3 destination = Vector3.zero;
	private bool m_isBeingUsed;
	private bool m_isGrowing = true;
	private bool m_isManual;
	private int m_direction = 0;
	private float m_staminaCost;
	private Vector3 m_lastPosition;
	private Vector3 m_newPosition;


	//Called if enough stamina and not already in use by other player
	public void Move(int dir)
    {
		CmdMove(dir);				
	}

	[Command(requiresAuthority = false)]
	public void CmdMove(int dir)
	{
		AudioManager.GetInstance().CmdPlaySoundEffectsLoop(ESound.slideMiddle, gameObject.transform.position);
		m_staminaCost = m_obstacleManager.m_staminaCost;
		GmStaminaManager.GetInstance().StartOverTimeCostCommand(m_staminaCost);
		RcpMove(dir);
	}
	
	[ClientRpc]
	public void RcpMove(int dir)
	{
		
		m_isManual = true;
		m_direction = dir;
	}

	//Called when there's no more stamina, or on click release
	public void StopMove()
    {
		CmdStopMove();
	}

	[Command(requiresAuthority = false)]
	public void CmdStopMove()
	{
		AudioManager.GetInstance().CmdStopSoundEffectsLoop(ESound.slideMiddle, gameObject.transform.position);
		GmStaminaManager.GetInstance().StopOverTimeCostCommand();
		RcpStopMove();

	}

	[ClientRpc]
	public void RcpStopMove()
	{
		
		m_isManual = false;
		m_direction = 0;
	}


	//Each rails move locally
	void Update()
    {	
		if (m_isManual)
        {
			if (GmStaminaManager.GetInstance().CanUseStaminaOverTime(m_staminaCost))
			{
				m_positionOnRailPercent += Time.deltaTime * m_direction * m_autoSpeed;
			}
			else
			{
				m_obstacleManager.ReleaseObstacle();
			}	
		}
        else if(m_isAuto)
        {
            if (m_isGrowing)
            {
				m_positionOnRailPercent += Time.deltaTime * m_autoSpeed;
			}
            else
            {
				m_positionOnRailPercent -= Time.deltaTime * m_autoSpeed;
			}


            if(m_positionOnRailPercent >= 100 || m_positionOnRailPercent <= 0)
            {
                if (m_isLooping)
                {
					if (m_positionOnRailPercent >= 100)
					{
						m_positionOnRailPercent = 0;
					}
                    else
                    {
						m_positionOnRailPercent = 100;
					}
				}
                else
                {
					m_isGrowing = !m_isGrowing;
				}
			}
        }

		m_positionOnRailPercent = Mathf.Clamp(m_positionOnRailPercent, 0, 100);

		int i = 0;
        foreach(int t in m_railPointsPercentage) 
        {
            if(t ==0 || t < m_positionOnRailPercent)
            {
				i++;
                continue;
			}
            else
            {
				destination = m_railPoints[i].position;
                break;
			}       
        }
      
		//Calculate movement
		Vector3 direction = destination - m_railPoints[i-1].position;

		float percentBetween2Points = m_positionOnRailPercent - m_railPointsPercentage[i - 1];
        percentBetween2Points = (percentBetween2Points * 100) / (m_railPointsPercentage[i] - m_railPointsPercentage[i-1]);
		m_lastPosition = m_objectToMove.transform.position;
			

        m_objectToMove.transform.position = m_railPoints[i - 1].position + (direction * (percentBetween2Points / 100));
		m_newPosition = m_objectToMove.transform.position;

    }

	public Vector3 GetLastPosition()
	{
		return m_lastPosition;
	}

	public Vector3 GetNewPosition()
	{
		return m_newPosition;
	}
}
