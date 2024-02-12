using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Body Parts")]
    [SerializeField] private List<SkinnedMeshRenderer> m_bodyPartsRenderer = new List<SkinnedMeshRenderer>();
    [SerializeField] private SkinnedMeshRenderer m_bigEyes;
    [SerializeField] private SkinnedMeshRenderer m_smallEyes;

    [Header("Materials")]
    [SerializeField] private Material m_redMaterial;
    [SerializeField] private Material m_greenMaterial;
    [SerializeField] private Material m_blueMaterial;
    [SerializeField] private Material m_yellowMaterial;
    [SerializeField] private Material m_purpleMaterial;

    [Header("Materials for invisible power")]
    [SerializeField] private Material m_blackEye;
    [SerializeField] private Material m_whiteEye;
    [SerializeField] private Material m_completeInvisible;

    [Header("Power Text Canvas")]
    [SerializeField] private TextMeshProUGUI m_powerText;
    [SerializeField] private float m_startFontSize;
    [SerializeField] private float m_endFontSize;
    [SerializeField] private float m_vfxDuration;
    private float m_currentTimer;
    private bool m_showPowerText = false;


    private EPowers m_currentPower = EPowers.none;
    private PlayerStateMachine m_playerFSM;
    private float m_lastData;

    private void Awake()
    {
        m_playerFSM = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        TextVFX();
    }

    private void TextVFX()
    {
        if (m_showPowerText)
        {
            m_currentTimer -= Time.deltaTime;

            if (m_currentTimer <= 0)
            {
                HidePowerText();
                return;
            }

            float lerp = m_currentTimer / m_vfxDuration;
            float TextFontSize = Mathf.Lerp(m_endFontSize, m_startFontSize, lerp);
            m_powerText.fontSize = TextFontSize;
        }
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
                SetRunnerColor(m_greenMaterial);
                ShowPowerText("Speed Power");
                break;

            case EPowers.invisibility:
                SetRunnerColor(m_purpleMaterial);
                SetRunnerInvisible(true);
                StartCoroutine(WaitAndBackToNormal(m_invisibilityPowerDuration));
                break;

            case EPowers.jump:     
                m_lastData = m_playerFSM.JumpIntensity;
                m_playerFSM.SetJumpForce(m_jumpForceDuringPower);
                StartCoroutine(WaitAndBackToNormal(m_jumpPowerDuration));
                SetRunnerColor(m_yellowMaterial);
                ShowPowerText("Jump Power");
                break;

            case EPowers.stamina:
                m_lastData = m_playerFSM.StaminaPlayer.GetStaminaMultiplier();
                m_playerFSM.StaminaPlayer.SetStaminaMultiplier(m_staminaMultiplierDuringPower);
                StartCoroutine(WaitAndBackToNormal(m_staminaPowerDuration));
                SetRunnerColor(m_blueMaterial);
                ShowPowerText("Stamina Power");
                break;

            case EPowers.blocker:
                break;

            case EPowers.none:
                break;

            default:
                break;
        }
    }

    /// <summary> Attend la durée du pouvoir pour tout réinitialiser </summary>
    IEnumerator WaitAndBackToNormal(float time)
    {

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
        else if (m_currentPower == EPowers.invisibility)
        {
            SetRunnerInvisible(false);
        }

        m_currentPower = EPowers.none;
        SetRunnerColor(m_redMaterial);
    }

    private void SetRunnerColor(Material material)
    {
        foreach (SkinnedMeshRenderer bodyPart in m_bodyPartsRenderer)
        {
            bodyPart.material = material;
        }
    }

    private void SetRunnerInvisible(bool isInvisible) 
    {
        foreach (SkinnedMeshRenderer bodyPart in m_bodyPartsRenderer)
        {
            bodyPart.enabled = !isInvisible;
        }
    }

    private void ShowPowerText(string powerName)
    {
        m_powerText.text = powerName;
        m_showPowerText = true;
        m_currentTimer = m_vfxDuration;
    }

    private void HidePowerText()
    {
        m_powerText.text = " ";
        m_showPowerText = false;
  
    }
}
