using UnityEngine;

public class EnableOnAnimationState : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string targetState = "Search";
    [SerializeField] private GameObject objectToToggle;

    private void Start()
    {
        if (objectToToggle)
            objectToToggle.SetActive(false);
    }

    void Update()
    {
        if (!animator || !objectToToggle) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool inState = state.IsName(targetState);

        objectToToggle.SetActive(inState);
    }
}
