using UnityEngine;

public class CameraCutTrigger : MonoBehaviour
{
    [SerializeField] private Transform cameraToMove;
    [SerializeField] private Transform targetCameraPosition;
    [SerializeField] private string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            cameraToMove.position = targetCameraPosition.position;
            cameraToMove.rotation = targetCameraPosition.rotation;
        }
    }
}
