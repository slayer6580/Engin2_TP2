using UnityEngine;
using UnityEngine.UI;

public class StaminaPlayer : MonoBehaviour
{
    public enum EStaminaState
    {
        recover,
        waitToRecover,
    }

    [SerializeField] private float m_maxStamina;
    [SerializeField] private float m_staminaRecoverOverTime;
    [SerializeField] private float m_recoverDelay;
    [SerializeField] private float m_runCostOverTime;
    [SerializeField] private float m_jumpCost;

    [SerializeField] private Image m_frontBarStaminaUI;

    private float m_currentStamina;
    private float m_currentRecoverCooldown;
    private EStaminaState m_currentState = EStaminaState.recover;

    private void Awake()
    {
        m_currentStamina = m_maxStamina;
    }

    private void Update()
    {
        if (m_currentState == EStaminaState.recover)
            RecoverStamina();
        else if (m_currentState == EStaminaState.waitToRecover)
            WaitToRecover();
    }

    // si tu appuis shift dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction RunCost()
    public bool CanRun()
    {
        return (m_currentStamina - (m_runCostOverTime * Time.deltaTime) >= 0);
    }
    // si tu appuis sur sauter dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction JumpCost()
    public bool CanJump()
    {
        return (m_currentStamina - m_jumpCost >= 0);
    }

    // Fonction a mettre dans le code du PlayerStateMachine quand CanRun es a true; 
    public void RunCost()
    {
        m_currentStamina -= m_runCostOverTime * Time.deltaTime;
        ResetTimer();
        StaminaCheck();
        SetStaminaUI();
    }

    // Fonction a mettre dans le code du PlayerStateMachine quand CanJump es a true;
    public void JumpCost()
    {
        m_currentStamina -= m_jumpCost;
        ResetTimer();
        StaminaCheck();
        SetStaminaUI();
    }

    // Dans update, pour récupérer de la stamina
    private void RecoverStamina()
    {
        if (m_currentStamina == m_maxStamina)
            return;

        m_currentStamina += m_staminaRecoverOverTime * Time.deltaTime;
        StaminaCheck();
        SetStaminaUI();
    }

    // Pour récupérer une fois le cooldown finit
    private void WaitToRecover()
    {
        m_currentRecoverCooldown -= m_recoverDelay * Time.deltaTime;

        if (m_currentRecoverCooldown < 0)
        {
            m_currentState = EStaminaState.recover;
        }
    }

    // S'assure de nos pas aller plus bas ou plus haut que le min/max
    private void StaminaCheck()
    {
        m_currentStamina = Mathf.Clamp(m_currentStamina, 0, m_maxStamina);
    }

    // recommence le timer pour la récupération de stamina
    private void ResetTimer()
    {
        m_currentRecoverCooldown = m_recoverDelay;
        m_currentState = EStaminaState.waitToRecover;
    }

    // Ca update le StaminaBar sur le UI;
    private void SetStaminaUI()
    {
        float currentStamina = (m_currentStamina / m_maxStamina);
        m_frontBarStaminaUI.rectTransform.localScale = new Vector3(currentStamina, 1, 1); ;
    }
}
