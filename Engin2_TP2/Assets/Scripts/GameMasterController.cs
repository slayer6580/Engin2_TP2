using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameMasterController : MonoBehaviour
{

	[SerializeField] private CinemachineVirtualCamera m_currentCamera;
    [SerializeField][Range (0.5f,5)] private float m_camMovementSpeed;
    private CinemachineTrackedDolly m_trackedDolly;

    private GameObject m_lastSelectedObject;
	Ray ray;
	RaycastHit hit;

	// Start is called before the first frame update
	void Start()
    {
        m_trackedDolly = m_currentCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
	}

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey("a"))
        {
            m_trackedDolly.m_PathPosition += 0.001f * m_camMovementSpeed;

		}
		if (Input.GetKey("d"))
		{
			m_trackedDolly.m_PathPosition -= 0.001f * m_camMovementSpeed;
		}

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit))
        {
            if(Input.GetMouseButtonDown(0))
            {
               if(hit.collider.name == "TerrainRotator")
                {
                    m_lastSelectedObject = hit.collider.gameObject;

					m_lastSelectedObject.GetComponent<ArenaRotation>().m_isSelected = true;

				}
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(m_lastSelectedObject != null)
            {
				m_lastSelectedObject.GetComponent<ArenaRotation>().m_isSelected = false;

			}
        }
	}
}
