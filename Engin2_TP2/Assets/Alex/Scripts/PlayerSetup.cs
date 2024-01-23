using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            return;
        }

        SetPlayerName();
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void SetPlayerName()
    {
        this.gameObject.name = "Player" + GetComponent<NetworkIdentity>().netId;
    }

}
