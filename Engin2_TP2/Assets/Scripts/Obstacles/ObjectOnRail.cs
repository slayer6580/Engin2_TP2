using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]

public class ObjectOnRail : NetworkBehaviour
{
	

	[SerializeField] private bool m_isAuto;
    [SerializeField] private bool m_isLooping;
	[SerializeField] private float m_autoSpeed;

	private bool m_isGrowing = true;
	private bool m_isManual;

	[SerializeField] private List<Transform> m_railPoints = new List<Transform>();
    [SerializeField] private List<int> m_railPointsPercentage = new List<int>();

    [SyncVar][SerializeField][Range (0,100)] private float m_positionOnRail;
    [SerializeField] private GameObject m_objectToMove;

    private Vector3 destination = Vector3.zero;

    private int m_direction = 0;

    public void Move(int dir)
    {
		MoveCommand(dir);
	}

	[Command(requiresAuthority = false)]
	public void MoveCommand(int dir)
	{
		m_isManual = true;
		m_direction = dir;
	}

	[Client]
	public void MoveRPC(int dir)
	{
		print("BB");
		m_isManual = true;
		m_direction = dir;
	}


	public void StopMove()
    {
		StopMoveCommand();
	}

	[Command(requiresAuthority = false)]
	public void StopMoveCommand()
	{
		m_isManual = false;
		m_direction = 0;
	}

	[Client]
	public void StopMoveRPC()
	{
		print("CC");
		m_isManual = false;
		m_direction = 0;
	}



	void Update()
    {
        if(m_isManual)
        {
			m_positionOnRail += Time.deltaTime * m_direction * m_autoSpeed;
		}
        else if(m_isAuto)
        {
            if (m_isGrowing)
            {
                m_positionOnRail += Time.deltaTime * m_autoSpeed;
			}
            else
            {
				m_positionOnRail -= Time.deltaTime * m_autoSpeed;
			}


            if(m_positionOnRail >= 100 || m_positionOnRail <=0)
            {
                if (m_isLooping)
                {
					if (m_positionOnRail >= 100)
					{
						m_positionOnRail = 0;
					}
                    else
                    {
						m_positionOnRail = 100;
					}
				}
                else
                {
					m_isGrowing = !m_isGrowing;
				}
					

			}
        }

		m_positionOnRail = Mathf.Clamp(m_positionOnRail, 0, 100);

		int i = 0;
        foreach(int t in m_railPointsPercentage) 
        {
            if(t ==0 || t < m_positionOnRail)
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
      
		Vector3 direction = destination - m_railPoints[i-1].position;

		float percentBetween2Points = m_positionOnRail - m_railPointsPercentage[i - 1];
        percentBetween2Points = (percentBetween2Points * 100) / (m_railPointsPercentage[i] - m_railPointsPercentage[i-1]);

		m_objectToMove.transform.position = m_railPoints[i - 1].position + (direction * (percentBetween2Points / 100));


	}
}
