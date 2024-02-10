using Mirror;
using UnityEngine;


public class PlayerPause : MonoBehaviour
{
    [SerializeField] private GameObject m_pausePanel;
	NetworkManager manager;
	[Scene][SerializeField] private string m_roomScene;


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

    public void PauseCanvasSetActive(bool value)
    {
        m_pausePanel.SetActive(value);
    }

    public void Lobby()
    {
		manager.ServerChangeScene(m_roomScene);
	}
}
