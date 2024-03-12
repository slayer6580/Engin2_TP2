using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [Header("Les composantes a ne pas reproduire sur les autres joueurs localement")]
    [SerializeField] Behaviour[] m_componentsToDisable;
    [SerializeField] GameObject m_canvas;
    [Header("Les composantes a desactiver en fin de partie")]
    [SerializeField] Behaviour[] m_endGameComponentsToDisable;
    [SerializeField] private Rigidbody m_rigidbody;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
        }
        else
        {
            ScoreManager.GetInstance().SetLocalPlayer(GetComponent<NetworkIdentity>());
           // AudioManager.GetInstance().GetComponent<AudioSource>().Play();
        }

        RunnerManager.GetInstance().SetParent(this.gameObject);  
        gameObject.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
    }

    /// <summary> Désactive tout les composantes qu'on veut pas répliquer sur les autres client sur son local </summary>  
    private void DisableComponents()
    {
        for (int i = 0; i < m_componentsToDisable.Length; i++)
        {
            m_componentsToDisable[i].enabled = false;
        }

        m_canvas.SetActive(false);
    }

    /// <summary> Désactive tout les composantes qu'on veut pas en fin de partie </summary>  
    public void DesactivateEndGameComponents()
    {
        foreach (Behaviour components in m_endGameComponentsToDisable)
        {
            components.enabled = false;
        }

        if (m_rigidbody != null)
        {
            m_rigidbody.isKinematic = true;
            m_rigidbody.useGravity = false;
        }
      
    }

}
