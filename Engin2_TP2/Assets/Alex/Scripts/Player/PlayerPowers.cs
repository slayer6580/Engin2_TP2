using System.Collections;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    public enum EPowers
    {
        speed,
        invisibility,
        jump,
        stamina,
        blocker,
        none
    }

    [Header("Speed Power")]
    [SerializeField] private float m_speedPowerDuration;
    [SerializeField] private float m_speedDuringPower;
  
    [Header("Invisibility Power")]
    [SerializeField] private float m_invisibilityPowerDuration;

    [Header("Jump Power")]
    [SerializeField] private float m_jumpPowerDuration;
    [SerializeField] private float m_jumpForceDuringPower;

    [Header("Stamina Power")]
    [SerializeField] private float m_staminaPowerDuration;

    [Header("Speed Power")]
    [SerializeField] private float m_blockerPowerDuration;

    private EPowers m_currentPower = EPowers.none;
    private PlayerStateMachine m_playerFSM;
    private float m_lastData;

    private void Awake()
    {
        m_playerFSM = GetComponent<PlayerStateMachine>();
    }

    public bool CanHavePower()
    {
        if (m_currentPower != EPowers.none)
        {
            return false;
        }
        return true;
    }

    public void GetPower(EPowers power)
    {
        m_currentPower = power;
        PowerEffects();
    }

    private void PowerEffects()
    {
        switch (m_currentPower)
        {
            case EPowers.speed:
                m_lastData = m_playerFSM.GroundSpeed;
                m_playerFSM.SetGroundSpeed(m_speedDuringPower);
                StartCoroutine(WaitAndBackToNormal(m_speedPowerDuration));
                break;

            case EPowers.invisibility:

                break;

            case EPowers.jump:     
                m_lastData = m_playerFSM.JumpIntensity;
                m_playerFSM.SetJumpForce(m_jumpForceDuringPower);
                StartCoroutine(WaitAndBackToNormal(m_jumpPowerDuration));
                break;

            case EPowers.stamina:

                break;

            case EPowers.blocker:

                break;

            case EPowers.none:

                break;
            default:
                break;
        }
    }

    IEnumerator WaitAndBackToNormal(float time)
    {
        yield return new WaitForSeconds(time);

        if (m_currentPower == EPowers.speed)
        {
            m_playerFSM.SetGroundSpeed(m_lastData);
        }
        else if (m_currentPower == EPowers.jump)
        {
            m_playerFSM.SetJumpForce(m_lastData);
        }

        m_currentPower = EPowers.none;
    }
}
