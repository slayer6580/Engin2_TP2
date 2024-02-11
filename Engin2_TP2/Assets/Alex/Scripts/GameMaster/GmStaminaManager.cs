using Mirror;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GmStaminaManager : NetworkBehaviour
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


	public delegate void ChangeStamina(float currentStamina, float maxStamina);
	public event ChangeStamina UpdateStamina;

	[SyncVar] private float m_currentStamina;

    public float CurrentStamina
    {
        get
        {
            return m_currentStamina;
        }
        set
        {
          
            m_currentStamina = value;
			UpdateStamina?.Invoke(CurrentStamina, m_maxStamina);
		}
    }



    private float m_currentRecoverCooldown;
    private float m_currentExaustedRecoverCooldown;
    private EStaminaState m_currentState = EStaminaState.recover;

    private float m_staminaLost;
    private bool m_isLosingStamina = false;

	private static GmStaminaManager s_instance = null;

	public static GmStaminaManager GetInstance()
	{
		return s_instance;
	}


   

	private void Awake()
	{
		CurrentStamina = m_maxStamina;
		if (s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Debug.LogError("Il y avait plus qu'une instance de GmStaminaManager dans la scène, FIX IT!");
			Destroy(this);
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
			CurrentStamina -= m_staminaLost * Time.deltaTime;
			ResetTimer();
			StaminaCheck();
		}
	}

    public bool CanUseStamina(float cost)
    {
		return CurrentStamina >= cost;
	}


    public bool CanUseStaminaOverTime(float cost)
    {
        return CurrentStamina > cost * Time.deltaTime;
    }


	[Command(requiresAuthority = false)]
	public void StartOverTimeCostCommand(float cost)
    {
		StartOverTimeCostRpc(cost);

	}

    [ClientRpc]
	public void StartOverTimeCostRpc(float cost)
	{
		m_staminaLost = cost;
		m_isLosingStamina = true;
	}

	[Command(requiresAuthority = false)]
	public void StopOverTimeCostCommand()
    {
        StopOverTimeCostRpc();

	}

    [ClientRpc]
	public void StopOverTimeCostRpc()
	{
		m_isLosingStamina = false;
	}

	[Command(requiresAuthority = false)]
	public void InstantCostCommand(float cost)
    {
       
        InstantCostRcp(cost);
	}

	[ClientRpc]
	public void InstantCostRcp(float cost)
	{
		CurrentStamina -= cost;
		ResetTimer();
		StaminaCheck();
	}




	/// <summary> Dans update, pour récupérer de la stamina  </summary>
	private void RecoverStamina()
    {
        if (CurrentStamina == m_maxStamina)
            return;

		CurrentStamina += m_staminaRecoverOverTime * Time.deltaTime;
        StaminaCheck();
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
        if (CurrentStamina < 0)
        {
            m_isLosingStamina = false;
            // Désactiver le click
        }

		CurrentStamina = Mathf.Clamp(CurrentStamina, 0, m_maxStamina);
    }

    /// <summary> recommence le timer pour la récupération de stamina </summary>
    private void ResetTimer()
    {
        m_currentRecoverCooldown = m_recoverDelay;
        m_currentState = EStaminaState.waitToRecover;
    }


    


}



