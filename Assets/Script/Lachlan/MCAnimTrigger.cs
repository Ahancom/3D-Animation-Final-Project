using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private string triggerName = "Searching"; // parameter name
    [SerializeField] private string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            playerAnimator.SetBool(triggerName, true); 
        }
    }
}
