using UnityEngine;
using UnityEngine.UI;

public class StaminaPlayer : MonoBehaviour
{
    public enum EStaminaState
    {
        recover,
        waitToRecover
    }

    [Header("Stamina Maximal")]
    [SerializeField] private float m_maxStamina;

    [Header("Le nombre de stamina par seconde récupérer")]
    [SerializeField] private float m_staminaRecoverOverTime;

    [Header("Le délai avant de récupérer de la stamina")]
    [SerializeField] private float m_recoverDelay;

    [Header("Le cout en stamina de courir par seconde")]
    [SerializeField] private float m_runCostOverTime;

    [Header("Le cout en stamina de sauter")]
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

    /// <summary> si tu appuis shift dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction RunCost() </summary>
    public bool CanRun()
    {
        return (m_currentStamina - (m_runCostOverTime * Time.deltaTime) >= 0);
    }
 
    /// <summary> si tu appuis sur sauter dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction JumpCost() </summary>
    public bool CanJump()
    {
        return (m_currentStamina - m_jumpCost >= 0);
    }

    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanRun es a true </summary>
    public void RunCost()
    {
        m_currentStamina -= m_runCostOverTime * Time.deltaTime;
        ResetTimer();
        StaminaCheck();
        SetStaminaUI();
    }
  
    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanJump es a true </summary>
    public void JumpCost()
    {
        m_currentStamina -= m_jumpCost;
        ResetTimer();
        StaminaCheck();
        SetStaminaUI();
    }
     
    /// <summary> Dans update, pour récupérer de la stamina  </summary>
    private void RecoverStamina()
    {
        if (m_currentStamina == m_maxStamina)
            return;

        m_currentStamina += m_staminaRecoverOverTime * Time.deltaTime;
        StaminaCheck();
        SetStaminaUI();
    }
   
    /// <summary> Un Décompte du coolDown pour récupérer de la stamina </summary>
    private void WaitToRecover()
    {
        m_currentRecoverCooldown -= m_recoverDelay * Time.deltaTime;

        if (m_currentRecoverCooldown < 0)
        {
            m_currentState = EStaminaState.recover;
        }
    }

    /// <summary> S'assure de nos pas aller plus bas ou plus haut que le min/max </summary>
    private void StaminaCheck()
    {
        m_currentStamina = Mathf.Clamp(m_currentStamina, 0, m_maxStamina);
    }

    /// <summary> recommence le timer pour la récupération de stamina </summary>
    private void ResetTimer()
    {
        m_currentRecoverCooldown = m_recoverDelay;
        m_currentState = EStaminaState.waitToRecover;
    }

    /// <summary> Update le StaminaBar sur le UI </summary>
    private void SetStaminaUI()
    {
        float currentStamina = (m_currentStamina / m_maxStamina);
        m_frontBarStaminaUI.rectTransform.localScale = new Vector3(currentStamina, 1, 1); ;
    }
}
