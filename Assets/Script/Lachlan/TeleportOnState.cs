using UnityEngine;

public class TeleportOnState : MonoBehaviour
{
    [SerializeField] private Animator animator;               // Animator to watch
    [SerializeField] private string targetState = "Search";   // State name to detect
    [SerializeField] private Transform objectToTeleport;      // Object to move
    [SerializeField] private Transform targetPosition;        // Target location

    private bool hasTeleported = false;

    void Update()
    {
        if (!animator || !objectToTeleport || !targetPosition) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(targetState) && !hasTeleported)
        {
            objectToTeleport.position = targetPosition.position;
            objectToTeleport.rotation = targetPosition.rotation;
            hasTeleported = true;
        }

        // Optional: reset if state is no longer active
        if (!state.IsName(targetState))
        {
            hasTeleported = false;
        }
    }
}
