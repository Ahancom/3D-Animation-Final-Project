using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    [SerializeField] private string triggeringTag = "Player";
    [SerializeField] private Transform objectToTeleport; 
    [SerializeField] private Transform targetPosition;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag) && objectToTeleport && targetPosition)
        {
            objectToTeleport.position = targetPosition.position;
            objectToTeleport.rotation = targetPosition.rotation; 
        }
    }
}
