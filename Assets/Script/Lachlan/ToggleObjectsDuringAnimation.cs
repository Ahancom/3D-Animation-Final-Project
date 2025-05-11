using UnityEngine;

public class ToggleObjectsOnState : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string targetState = "Grab";
    [SerializeField] private GameObject[] objectsToToggle;

    private bool isActive = false;

    void Update()
    {
        if (!animator) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool inTargetState = state.IsName(targetState);

        if (inTargetState && !isActive)
        {
            SetObjectsActive(true);
            isActive = true;
        }
        else if (!inTargetState && isActive)
        {
            SetObjectsActive(false);
            isActive = false;
        }
    }

    private void SetObjectsActive(bool value)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj) obj.SetActive(value);
        }
    }
}
