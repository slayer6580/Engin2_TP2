using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRotation : MonoBehaviour
{
	enum Point { North, East, South, West };

	[SerializeField] GameObject m_arena;
    [SerializeField] Point m_rotationPoint;

	public bool m_isSelected = false;

	[SerializeField] private float m_maxX;
	[SerializeField] private float m_minX;

	private Vector3 m_currentRotation = Vector3.zero;



	public void PlaceAtLimit(float toCheck, Vector3 replaceBy, float limitA, float limitB, float replaceValue)
	{
		if (toCheck > limitA && toCheck < limitB)
		{
			m_arena.transform.eulerAngles = new Vector3(replaceValue * replaceBy.x, 0, replaceValue * replaceBy.z);
		}	
	}


	// Update is called once per frame
	void Update()
    {
		
		if (m_isSelected)
		{

			switch(m_rotationPoint)
			{
				case Point.North:
					m_currentRotation = new Vector3(1,0,0);
					MoveOnXAxis();
					break;

				case Point.South:
					m_currentRotation = new Vector3(-1, 0, 0);
					MoveOnXAxis();
					break;

				case Point.East:
					m_currentRotation = new Vector3(0, 0, 1);
					MoveOnZAxis();
					break;

				case Point.West:
					m_currentRotation = new Vector3(0, 0, -1);
					MoveOnZAxis();
					break;

			}
		}
    }

	public void MoveOnXAxis()
	{
		if (Input.GetAxis("Mouse Y") > 0)
		{

			if (Mathf.Round(m_arena.transform.eulerAngles.x) <= m_maxX || Mathf.Round(m_arena.transform.eulerAngles.x) >= 360 - m_maxX)
			{
				m_arena.transform.eulerAngles -= m_currentRotation;
			}

			ResetZ();
		}
		else if (Input.GetAxis("Mouse Y") < 0)
		{

			if (Mathf.Round(m_arena.transform.eulerAngles.x) >= 360 + m_minX || (Mathf.Round(m_arena.transform.eulerAngles.x) <= m_maxX))
			{
				m_arena.transform.eulerAngles += m_currentRotation; // -1,0,0 SOUTH
			}

			ResetZ();
		}

		//Replace if too far
		PlaceAtLimit(m_arena.transform.eulerAngles.x, new Vector3(1, 0, 0), m_maxX, 300, m_maxX);
		PlaceAtLimit(m_arena.transform.eulerAngles.x, new Vector3(1, 0, 0), 300, 360 + m_minX, m_minX);
	}

	public void MoveOnZAxis()
	{
		if (Input.GetAxis("Mouse Y") > 0)
		{
			if (Mathf.Round(m_arena.transform.eulerAngles.z) >= 360 + m_minX || Mathf.Round(m_arena.transform.eulerAngles.z) <= m_maxX)
			{
				m_arena.transform.eulerAngles += m_currentRotation;
			}
			//If we move on Z, we slowly reset the X so the balance don't become too weird
			ResetX();
		}
		else if (Input.GetAxis("Mouse Y") < 0)
		{
			if (Mathf.Round(m_arena.transform.eulerAngles.z) <= m_maxX || Mathf.Round(m_arena.transform.eulerAngles.z) >= 360 - m_maxX)
			{
				m_arena.transform.eulerAngles -= m_currentRotation;
			}
			ResetX();
		}

		//Replace if too far
		PlaceAtLimit(m_arena.transform.eulerAngles.z, new Vector3(0, 0, 1), m_maxX, 300, m_maxX);
		PlaceAtLimit(m_arena.transform.eulerAngles.z, new Vector3(0, 0, 1), 300, 360 + m_minX, m_minX);
	}

	public void ResetX()
	{
		if (m_arena.transform.eulerAngles.x > 0 && m_arena.transform.eulerAngles.x < 300)
		{
			m_arena.transform.eulerAngles += new Vector3(-1, 0, 0);
		}

		if (m_arena.transform.eulerAngles.x < 0 || m_arena.transform.eulerAngles.x > 300)
		{
			m_arena.transform.eulerAngles += new Vector3(1, 0, 0);
		}
	}

	public void ResetZ()
	{
		if (m_arena.transform.eulerAngles.z > 0 && m_arena.transform.eulerAngles.z < 300)
		{
			m_arena.transform.eulerAngles += new Vector3(0, 0, -1);
		}

		if (m_arena.transform.eulerAngles.z < 0 || m_arena.transform.eulerAngles.z > 300)
		{
			m_arena.transform.eulerAngles += new Vector3(0, 0, 1);
		}
	}
}
