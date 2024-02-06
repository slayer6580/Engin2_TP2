using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGamemaster : NetworkBehaviour
{
    public enum EStaminaState
    {
        recover,
        waitToRecover,
    }

    [Header("Stamina Maximal")]
    [SerializeField] private float m_maxStamina;

    [Header("Le nombre de stamina par seconde récupérer")]
    [SerializeField] private float m_staminaRecoverOverTime;

    [Header("Le délai avant de récupérer de la stamina")]
    [SerializeField] private float m_recoverDelay;

    [SerializeField] private Image m_frontBarStaminaUI;

    private float m_currentStamina;
    private float m_currentRecoverCooldown;
    private float m_currentExaustedRecoverCooldown;
    private EStaminaState m_currentState = EStaminaState.recover;

    private float m_staminaLost;
    private bool m_isLosingStamina = false;

    private void Awake()
    {
        m_currentStamina = m_maxStamina;

        //TODO , regarder si ca marche
        if (!isLocalPlayer)
        {
            this.enabled = false;
        }

    }

    private void Update()
    {
     
        if (m_currentState == EStaminaState.recover)
            RecoverStamina();
        else if (m_currentState == EStaminaState.waitToRecover)
            WaitToRecover();

        if (m_isLosingStamina)
        {
            m_currentStamina -= m_staminaLost * Time.deltaTime;
            ResetTimer();
            StaminaCheck();
            SetStaminaUI();
        }
    }

    /// <summary> si tu veux courir ou sauter dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction RunCost() ou JumpCost </summary>
    public bool CanUseStamina(float cost)
    {
        return m_currentStamina >= cost;
    }

    public bool CanUseStaminaOverTime(float cost)
    {
        return m_currentStamina > cost * Time.deltaTime;
    }

    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanRun es a true </summary>
    public void UpdateCost(float cost)
    {
        m_staminaLost = cost;
        m_isLosingStamina = true;
    }

    public void StopStaminaCost()
    {
        m_isLosingStamina = false;
    }
    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanJump es a true </summary>
    public void InstantCost(float cost)
    {
        m_currentStamina -= cost;
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
        m_currentRecoverCooldown -= Time.deltaTime;

        if (m_currentRecoverCooldown < 0)
        {
            m_currentState = EStaminaState.recover;
        }
    }

    /// <summary> S'assure de nos pas aller plus bas ou plus haut que le min/max </summary>
    private void StaminaCheck()
    {
        if (m_currentStamina < 0)
        {
            m_isLosingStamina = false;
            // Désactiver le click
        }

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

