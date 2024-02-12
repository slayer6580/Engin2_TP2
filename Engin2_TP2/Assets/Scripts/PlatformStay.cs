using UnityEngine;

public class PlatformStay : MonoBehaviour
{
    private Transform m_playerTransform = null;
    [SerializeField] private ObjectOnRail m_platform;

    private void Update()
    {
        if (m_playerTransform != null)
        {
            Vector3 translation = m_platform.GetNewPosition() - m_platform.GetLastPosition();
            m_playerTransform.Translate(translation);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       Transform player = other.gameObject.GetComponent<PlayerStateMachine>().gameObject.transform;

        if (player == null)
            return;

        m_playerTransform = player;       
    }

    private void OnTriggerExit(Collider other)
    {
        Transform player = other.gameObject.GetComponent<PlayerStateMachine>().gameObject.transform;

        if (player == null)
            return;

        m_playerTransform = null;
    }


}
