using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterStamina : MonoBehaviour
{
	[SerializeField] private Image m_frontBarStaminaUI;

	private void Start()
	{
		GmStaminaManager staminaManager = GmStaminaManager.GetInstance();
		if(staminaManager != null)
		{
			staminaManager.UpdateStamina += SetStaminaUI;
		}
		else
		{
			print("Error: HELP");
		}
		
	}
	/// <summary> Update le StaminaBar sur le UI </summary>
	private void SetStaminaUI(float currentStamina, float maxStamina)
	{
		float updatedStamina = (currentStamina / maxStamina);
		m_frontBarStaminaUI.rectTransform.localScale = new Vector3(updatedStamina, 1, 1); ;
	}
}
