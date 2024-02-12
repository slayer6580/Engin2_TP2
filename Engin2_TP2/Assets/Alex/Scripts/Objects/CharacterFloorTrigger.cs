using UnityEngine;

public class CharacterFloorTrigger : MonoBehaviour
{
    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if (!IsOnFloor && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Vient de toucher le sol");
            IsOnFloor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Vient de quitter le sol");
            IsOnFloor = false;
        }
    }
}
