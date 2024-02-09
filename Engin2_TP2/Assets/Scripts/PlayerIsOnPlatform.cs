using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    GameObject gameManager;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }
    private void OnTriggerEnter(Collider other)
    {
        
        PlayerStateMachine stateMachine = other.GetComponent<PlayerStateMachine>();
        //Debug.Log("Collision baby!");
        
        if (stateMachine != null)
        {
            // Set the player's parent to this platform
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        PlayerStateMachine stateMachine = other.GetComponent<PlayerStateMachine>();
        //Debug.Log("No more Collision baby!");

        if (stateMachine != null && gameManager != null) 
        {
            other.transform.SetParent(gameManager.transform);
        }
        else if (stateMachine != null)
        {
            
            other.transform.SetParent(null);
        }
    }
}
