using Mirror;
using UnityEngine;


public class PlayerPause : NetworkBehaviour
{
    [SerializeField] private GameObject m_pausePanel;
	NetworkManager manager;


	void Awake()
	{
		manager = GetComponent<NetworkManager>();
	}

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
		NetworkManager.singleton.gameObject.GetComponent<MenuButton>().ToMainMenu();
	}
}
