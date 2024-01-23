using Mirror;
using UnityEngine;


public class CharacterColor : NetworkBehaviour
{
    private enum EColor { Red, Blue, Green }
    private Material m_material = null;

    [Header("Mettre toute les parties du corps dans cette array")]
    [SerializeField] private SkinnedMeshRenderer[] m_bodyParts;

    private void Start()
    {
        SetMaterialColor();
        Cmd_SetColorForAll();
    }

    [Client] // only runs code on the client
    private void SetMaterialColor()
    {
        int index = StartPointManager.GetInstance().SetParentAndColor(this.gameObject);
        GetMaterialColor(index);
    }

    [Command]
    private void Cmd_SetColorForAll()
    {
        Rpc_SetPlayersColor();
    }

    [ClientRpc]
    private void Rpc_SetPlayersColor()
    {
        if (m_material == null)
        {
            Debug.LogError("Il n'y a pas de material référencer au client, FIX IT!");
            return;
        }

        for (int i = 0; i < m_bodyParts.Length; i++)
        {
            m_bodyParts[i].material = m_material;
        }
    }

    public void GetMaterialColor(int index)
    {
        EColor color;

        if (index == 0)
            color = EColor.Red;
        else if (index == 1)
            color = EColor.Blue;
        else if (index == 2)
            color = EColor.Green;
        else
            color = EColor.Red;

        m_material = Resources.Load<Material>("Colors/Character" + color.ToString());
    }



}
