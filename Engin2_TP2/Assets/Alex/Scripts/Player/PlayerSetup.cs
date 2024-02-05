using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    [SerializeField] GameObject m_canvas;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();          
        }

        RunnerManager.GetInstance().SetParent(this.gameObject);
        gameObject.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
    }

    /// <summary> Désactive tout les composantes qu'on veut pas répliquer sur les autres client sur son local </summary>  
    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }

        m_canvas.SetActive(false);
    }

}
