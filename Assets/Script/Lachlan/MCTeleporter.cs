using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            other.transform.position = teleportTarget.position;
        }
    }
}
