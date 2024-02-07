using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_lookAt; //empty GameObject in main character
    [SerializeField]
    private Transform m_mainPlayer; // main character
    [SerializeField]
    private float m_rotationSpeedY = 1.0f;
    [SerializeField]
    private float m_rotationSpeedX = 1.0f;
    [SerializeField]
    private Vector2 m_clampingYRotationValues = Vector2.zero;
    private float m_desiredDistance = 10.0f;
    [SerializeField]
    private float m_lerpSpeed = 0.05f;
    [SerializeField]
    private Vector2 m_zoomClampValues = new Vector2(2.0f, 15.0f);

    private CinemachineVirtualCamera m_mainVirtualCamera;

    private void Awake()
    {
        m_mainVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();  
    }

    private void FixedUpdate()
    {
         FixedUpdateCameraLerp();
    }

    private void FixedUpdateCameraLerp()
    {  
        Cinemachine3rdPersonFollow cameraDistance = m_mainVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cameraDistance.CameraDistance = Mathf.Lerp(cameraDistance.CameraDistance, m_desiredDistance, m_lerpSpeed);       
    }

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeedX;
        m_lookAt.transform.Rotate(new Vector3(0, currentAngleX, 0), Space.World);
    }

    private void UpdateVerticalMovements()
    {
        float currentAngleY = -Input.GetAxis("Mouse Y") * m_rotationSpeedY;
        float eulersAngleX = transform.rotation.eulerAngles.x;

        float comparisonAngle = eulersAngleX + currentAngleY;
        comparisonAngle = ClampAngle(comparisonAngle);

        if ((currentAngleY < 0 && comparisonAngle < m_clampingYRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingYRotationValues.y))
        {
            return;
        }  

        m_lookAt.transform.Rotate(new Vector3(currentAngleY, 0, 0), Space.Self);
    }

    private void UpdateCameraScroll()
    {
        m_desiredDistance += -Input.mouseScrollDelta.y;
        m_desiredDistance = Mathf.Clamp(m_desiredDistance, m_zoomClampValues.x, m_zoomClampValues.y);
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
