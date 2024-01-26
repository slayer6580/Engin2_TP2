using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    [SerializeField] GameObject m_canvas;
    [SyncVar] [HideInInspector] public string m_name = "";

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            m_canvas.SetActive(false);
            return;
        }

        PlayersManager.GetInstance().Cmd_AddPlayer(this.gameObject);
        PlayersManager.GetInstance().SetParent(this.gameObject);
    }


    [ClientRpc]
    public void Rpc_SetNameOnConnect()
    {
        gameObject.name = m_name;     
    }


    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

}
