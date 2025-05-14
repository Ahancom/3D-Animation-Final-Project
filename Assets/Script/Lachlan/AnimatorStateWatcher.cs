using UnityEngine;

public class AnimatorStateWatcher : MonoBehaviour
{
    [SerializeField] private Animator sourceAnimator;
    [SerializeField] private string sourceStateName = "Walk";

    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string targetBoolName = "Sitting";

    [SerializeField] private bool onlySetOnce = false;

    private bool alreadySet = false;
    private bool wasInState = false;

    void Update()
    {
        if (!sourceAnimator || !targetAnimator) return;

        AnimatorStateInfo stateInfo = sourceAnimator.GetCurrentAnimatorStateInfo(0);
        bool inState = stateInfo.IsName(sourceStateName);

        if (inState && !wasInState)
        {
            Debug.Log("Entered state, disabling target bool");
            targetAnimator.SetBool(targetBoolName, false);
            if (onlySetOnce) alreadySet = true;
        }
        else if (!inState && wasInState && !onlySetOnce)
        {
            Debug.Log("Exited state, enabling target bool");
            targetAnimator.SetBool(targetBoolName, true);
        }

        wasInState = inState;
    }
}
