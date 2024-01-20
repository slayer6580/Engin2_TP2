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

	void Start()
    {
        m_trackedDolly = m_currentCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
	}

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
               if(hit.collider.transform.parent.name == "ArenaRotationManager")        //To Replace by if Interactable
                {
                    if (hit.collider.gameObject.GetComponent<ArenaRotation>().IsActivated)
                    {
						m_lastSelectedObject = hit.collider.gameObject;
						m_lastSelectedObject.GetComponent<ArenaRotation>().IsSelected = true;
					}
				}
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(m_lastSelectedObject != null)
            {
				m_lastSelectedObject.GetComponent<ArenaRotation>().IsSelected = false;

			}
        }
	}
}
