using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] private float m_rotatingSpeed;
    [SerializeField] private GameObject m_toRotate;
    [SerializeField] private bool m_isRotating;
	public void Rotate()
    {
        m_isRotating = true;
		
    }

    public void StopRotating()
    {
		m_isRotating = false;
	}

	public void Update()
	{
		if (m_isRotating)
		{
			m_toRotate.transform.Rotate(Vector3.up, m_rotatingSpeed * Time.deltaTime);
		}
	}
}
