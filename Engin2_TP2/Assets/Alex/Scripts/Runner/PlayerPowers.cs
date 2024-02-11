using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
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
    [SerializeField] private float m_speedMultiplierDuringPower;
  
    [Header("Invisibility Power")]
    [SerializeField] private float m_invisibilityPowerDuration;

    [Header("Jump Power")]
    [SerializeField] private float m_jumpPowerDuration;
    [SerializeField] private float m_jumpForceDuringPower;

    [Header("Stamina Power")]
    [SerializeField] private float m_staminaPowerDuration;
    [SerializeField] private float m_staminaMultiplierDuringPower;

    private EPowers m_currentPower = EPowers.none;
    private PlayerStateMachine m_playerFSM;
    private float m_lastData;

    private void Awake()
    {
        m_playerFSM = GetComponent<PlayerStateMachine>();
    }

    /// <summary> Retourne vrai si le joueur na pas de pouvoir actuel </summary>
    public bool CanHavePower()
    {
        if (m_currentPower != EPowers.none)
        {
            return false;
        }
        return true;
    }

    /// <summary> Prendre un pouvoir </summary>
    public void GetPower(EPowers power)
    {
        m_currentPower = power;
        PowerEffects();
    }

    /// <summary> Faire l'effet du pouvoir actuel </summary>
    private void PowerEffects()
    {
        switch (m_currentPower)
        {
            case EPowers.speed:
                m_lastData = m_playerFSM.SpeedMultiplier;
                m_playerFSM.SetSprintMultiplier(m_speedMultiplierDuringPower);
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
                m_lastData = m_playerFSM.StaminaPlayer.GetStaminaMultiplier();
                m_playerFSM.StaminaPlayer.SetStaminaMultiplier(m_staminaMultiplierDuringPower);
                StartCoroutine(WaitAndBackToNormal(m_staminaPowerDuration));
                break;

            case EPowers.blocker:

                break;

            case EPowers.none:

                // Code Review:
                // En théorie, ça ne devrait jamais être appelé... 
                // On pourrait faire du proofing en rajoutant une fonction qui clean tout
               

                break;
            default:
                break;
        }
    }

    /// <summary> Attend la durée du pouvoir pour tout réinitialiser </summary>
    IEnumerator WaitAndBackToNormal(float time)
    {

        // Code Review:
        // On pourrait sauver potentiellement beaucoup de coroutines avec un PowerupManager qui gère les powerups sur le map.
        // On aurait un seul timer et à chaque x frames, le manager check ce qu'il doit respawn
      

        yield return new WaitForSeconds(time);

        if (m_currentPower == EPowers.speed)
        {
            m_playerFSM.SetSprintMultiplier(m_lastData);
        }
        else if (m_currentPower == EPowers.jump)
        {
            m_playerFSM.SetJumpForce(m_lastData);
        }
        else if (m_currentPower == EPowers.stamina)
        {
            m_playerFSM.StaminaPlayer.SetStaminaMultiplier(m_lastData);
        }

        m_currentPower = EPowers.none;
    }
}
