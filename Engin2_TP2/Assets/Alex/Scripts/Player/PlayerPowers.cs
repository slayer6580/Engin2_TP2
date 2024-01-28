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
        // Code Review:
        // Je n'ai pas réussi à voir à quoi ce script est attaché?
        // Il faudrait vraiment un PowerupManager (ou au moins un GameManager) sinon je vois quand ce awake() est appelé
        // Aussi il faut rajouter quelque chose si le PlayerStateMachine n'est pas là
        //

        m_playerFSM = GetComponent<PlayerStateMachine>();
    }

    /// <summary> Retourne vrai si le joueur na pas de pouvoir actuel </summary>
    public bool CanHavePower()
    {

        // Code Review:
        // Bonne place pour un opérateur terniaire
        // m_currentPower !=EPowers.none ? false : true;
        //

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

                // Code Review:
                // En théorie, ça ne devrait jamais être appelé... 
                // On pourrait faire du proofing en rajoutant une fonction qui clean tout
                //

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
        //

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
