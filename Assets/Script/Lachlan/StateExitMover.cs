using UnityEngine;

public class CutsceneStep : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string stateToWaitFor = "Search";       
    [SerializeField] private string triggerToFire = "Thinking";   

    [Header("Moves")]
    [SerializeField] private Transform playerToMove;
    [SerializeField] private Transform newPlayerPos;
    [SerializeField] private Transform cameraToMove;
    [SerializeField] private Transform newCameraPos;

    [Header("Optional")]
    [SerializeField] private bool moveOnlyOnce = true;

    private bool moved = false;

    void Update()
    {
        if (!animator) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(stateToWaitFor) && state.normalizedTime >= 1f)
        {
            if (!moved || !moveOnlyOnce)
            {
                playerToMove.position = newPlayerPos.position;
                playerToMove.rotation = newPlayerPos.rotation;

                cameraToMove.position = newCameraPos.position;
                cameraToMove.rotation = newCameraPos.rotation;

                animator.SetTrigger(triggerToFire);
                moved = true;
            }
        }
    }
}
