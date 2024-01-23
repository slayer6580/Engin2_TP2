using Mirror;
using UnityEngine;

public class CharacterColor : NetworkBehaviour
{
    private enum EColor { Red, Blue, Green }

    [Header("Mettre toute les parties du corps dans cette array")] 
    [SerializeField] private SkinnedMeshRenderer[] m_bodyParts;

    private void Start()
    {
       int index = StartPointManager.GetInstance().SetParentAndColor(this.gameObject);
       SetCharacterColor(index);
    }

    [Client] // only runs code on the client
    public void SetCharacterColor(int index)
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

        Material _material = Resources.Load<Material>("Colors/Character" + color.ToString());

        for (int i = 0; i < m_bodyParts.Length; i++)
        {
            m_bodyParts[i].material = _material;
        }
    }

}
