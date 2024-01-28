using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// G�re les �tapes de jeux, les points, les mises de joueur aux bonnes places, etc.

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeLeftInGame;
    public enum GameState 
    { 
        start,
        inGame,
        pause,
        end,
        none
    } // Nous n'impl�menterons pas de pause, je crois

    private GameState m_currentState = GameState.none;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) // Singleton; � revoir avec le multiplayer? Il doit runner sur le serveur...
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the GameManager across scenes.
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one GameManager.
        }
    }
    public void StartGame()
    { 
        m_currentState = GameState.start;
    }
    public void EndGame()
    {
        m_currentState = GameState.end;

        // Afficher quelle �quipe a gagn�
        // Apr�s un d�lai, retourner au lobby
        // Cleanup le stuff
    }
    public void InGame()
    {
        m_currentState = GameState.inGame;
        // Les joueurs peuvent bouger, etc etc.
    }
    public void UpdateScore()
    { 
        // doit �tre reli� au GUI
    }
}
