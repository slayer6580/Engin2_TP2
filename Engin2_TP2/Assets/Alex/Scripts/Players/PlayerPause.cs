using UnityEngine;


public class PlayerPause : MonoBehaviour
{
    [SerializeField] private GameObject m_pausePanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pausePanel.activeSelf)
        {
            PauseCanvasSetActive(true);
        }
    }

    /// <summary> Pour activer ou désactiver le pause Canvas </summary>
    public void PauseCanvasSetActive(bool value)
    {
        m_pausePanel.SetActive(value);
    }

    /// <summary> Pour retourner au lobby </summary>
    public void Lobby()
    {
        // TODO 
        // Rajouter le code quand un des joueur va cliquer sur le bouton Lobby pour redémarrer la partie au lobby.
    }
}
