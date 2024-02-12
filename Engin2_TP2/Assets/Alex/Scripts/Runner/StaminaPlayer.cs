using UnityEngine;
using UnityEngine.UI;

public class StaminaPlayer : MonoBehaviour
{
    public enum EStaminaState
    {
        recover,
        waitToRecover,
        exhausted,
        exhaustedRecover
    }

    [Header("Stamina Maximal")]
    [SerializeField] private float m_maxStamina;

    [Header("Le nombre de stamina par seconde récupérer")]
    [SerializeField] private float m_staminaRecoverOverTime;

    [Header("Le nombre de stamina par seconde récupérer quand on est épuisé")]
    [SerializeField] private float m_exaustedStaminaRecoverOverTime;

    [Header("La quantité de stamina pour ne pas etre épuisé")]
    [SerializeField] private float m_notExaustedStamina;

    [Header("Le délai avant de récupérer de la stamina")]
    [SerializeField] private float m_recoverDelay;

    [Header("Le délai avant de récupérer de la stamina quand on s'épuise")]
    [SerializeField] private float m_exaustedRecoverDelay;

    [Header("Le cout en stamina de courir par seconde")]
    [SerializeField] private float m_runCostOverTime;

    [Header("Le cout en stamina de sauter")]
    [SerializeField] private float m_jumpCost;

    [Header("Stamina Multiplier Power")]
    [SerializeField] private float m_staminaMultiplier = 1.0f;

    [SerializeField] private Image m_frontBarStaminaUI;

    private float m_currentStamina;
    private float m_currentRecoverCooldown;
    private float m_currentExaustedRecoverCooldown;
    private EStaminaState m_currentState = EStaminaState.recover;

    private bool m_notInExhaustionMode => m_currentState != EStaminaState.exhausted && m_currentState != EStaminaState.exhaustedRecover;

    private void Awake()
    {
       ResetStamina();
    }

    private void Update()
    {
        if (m_currentState == EStaminaState.recover)
            RecoverStamina();
        else if (m_currentState == EStaminaState.waitToRecover)
            WaitToRecover();
        else if (m_currentState == EStaminaState.exhausted)
            TakeABreak();
        else if (m_currentState == EStaminaState.exhaustedRecover)
            ExhaustedRecoverStamina();

    }

    /// <summary> si tu veux courir ou sauter dans PlayerStateMachine et que ca aussi c'est vrai, tu peux rouler la fonction RunCost() ou JumpCost </summary>
    public bool CanUseStamina()
    {
        return m_currentStamina > 0 && m_notInExhaustionMode; 
    }

    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanRun es a true </summary>
    public void RunCost()
    {
        m_currentStamina -= m_runCostOverTime * Time.fixedDeltaTime * m_staminaMultiplier;
        ResetTimer();
        StaminaCheck();
        SetStaminaUI();
    }
  
    /// <summary> Fonction a mettre dans le code du PlayerStateMachine quand CanJump es a true </summary>
    public void JumpCost()
    {
        m_currentStamina -= m_jumpCost * m_staminaMultiplier;
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

    /// <summary> Fonction dans update qui attend d'avoir un min de stamina pour recommencer l'utilisation du stamina pour le runner </summary>
    private void ExhaustedRecoverStamina()
    {  
        m_currentStamina += m_exaustedStaminaRecoverOverTime * Time.deltaTime;

        if (m_currentStamina >= m_notExaustedStamina)
        {
            m_currentState = EStaminaState.recover;
            m_frontBarStaminaUI.color = Color.green;
        }

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

	/// <summary> Pour recommencer à gagner de la stamina sans attendre le cooldown </summary>
	public void GiveBackStamina()
    {
        m_currentStamina = m_maxStamina;

	}

    /// <summary> Un Décompte du coolDown pour récupérer de la stamina apres un épuisement </summary>
    private void TakeABreak()
    {
        m_currentExaustedRecoverCooldown -= Time.deltaTime;

        if (m_currentExaustedRecoverCooldown < 0)
        {
            m_currentState = EStaminaState.exhaustedRecover;
        }
    }

    /// <summary> S'assure de nos pas aller plus bas ou plus haut que le min/max </summary>
    private void StaminaCheck()
    {
        if (m_currentStamina < 0)
        {
            Exausted();
        }

        m_currentStamina = Mathf.Clamp(m_currentStamina, 0, m_maxStamina);
    }

    /// <summary> recommence le timer pour la récupération de stamina </summary>
    private void ResetTimer()
    {
        m_currentRecoverCooldown = m_recoverDelay;
        m_currentState = EStaminaState.waitToRecover;
    }

    /// <summary> recommence le timer pour la récupération de stamina aprés épuisement </summary>
    private void Exausted()
    {
        m_currentState = EStaminaState.exhausted;
        m_frontBarStaminaUI.color = Color.red;
        m_currentExaustedRecoverCooldown = m_exaustedRecoverDelay;
    }

    /// <summary> Update le StaminaBar sur le UI </summary>
    public void SetStaminaUI()
    {
        float currentStamina = (m_currentStamina / m_maxStamina);
        m_frontBarStaminaUI.rectTransform.localScale = new Vector3(currentStamina, 1, 1);
    }

    /// <summary> Reset le stamina du joueur </summary>
    public void ResetStamina()
    {
        m_currentStamina = m_maxStamina;
    }

    public void SetStaminaMultiplier(float multiplier)
    {
        m_staminaMultiplier = multiplier;
    }

    public float GetStaminaMultiplier()
    {
        return m_staminaMultiplier;
    }
}
