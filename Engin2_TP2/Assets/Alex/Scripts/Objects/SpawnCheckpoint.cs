using UnityEngine;

public class SpawnCheckpoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward.normalized * 2);
    }
}
