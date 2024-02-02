using UnityEngine;
using System.Collections;

public class Bumper1IdleMoving : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveDuration = 5f;
    private float maxMove = 12f;

    //Attention! L'object bump, mais n'est pas "bumpable" Il doit rester sur son rail invisible!
    void Awake()
    {
        
        startPosition = transform.position;
       
        targetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + maxMove);
        // Start the movement coroutine
        StartCoroutine(MoveBumperBackAndForth());
    }

    IEnumerator MoveBumperBackAndForth()
    {
       
        yield return MoveOverSeconds(targetPosition, moveDuration);
        yield return MoveOverSeconds(startPosition, moveDuration);
        
        // Et on répète:
        StartCoroutine(MoveBumperBackAndForth());
    }

    IEnumerator MoveOverSeconds(Vector3 target, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, target, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = target; 
    }
}