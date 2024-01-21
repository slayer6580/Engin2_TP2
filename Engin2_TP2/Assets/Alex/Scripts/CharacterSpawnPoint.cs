using UnityEngine;

[RequireComponent (typeof(CharacterColor))]
public class CharacterSpawnPoint : MonoBehaviour
{
    private Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        spawnPosition = StartPointManager.GetInstance().GetStartPointAndSetColor(this.gameObject);
        Invoke("SetInitialPosition", 0.001f);
    }

    private void SetInitialPosition()
    {
        transform.position = spawnPosition;
    }

    public void GoToStartPoint()
    {
        transform.position = spawnPosition;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        // On va devoir rajouter des choses au besoin
    }

 
}
