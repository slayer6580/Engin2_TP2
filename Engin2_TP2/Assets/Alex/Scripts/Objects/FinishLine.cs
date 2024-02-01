using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterSpawnPoint character = other.GetComponent<CharacterSpawnPoint>();

        if (character == null)
            return;

        character.GoToSpawnPoint();
        GameManager.GetInstance().CMD_ScoreRunner();

    }
}
