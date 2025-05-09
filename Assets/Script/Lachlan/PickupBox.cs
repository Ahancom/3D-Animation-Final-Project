using UnityEngine;

public class AttachDuringAnimation : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private string targetState = "Grab";
    [SerializeField, Range(0f, 1f)] private float attachTime = 0.5f;

    [Header("Attachment Targets")]
    [SerializeField] private GameObject objectToAttach;
    [SerializeField] private Transform handTarget;

    [Header("Offset From Hand")]
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    [Header("On Detach")]
    [SerializeField] private bool destroyOnDetach = false;
    [SerializeField] private Transform moveToOnDetach;

    private bool hasAttached = false;
    private bool wasInState = false;

    void Update()
    {
        if (!animator || !objectToAttach || !handTarget) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool inTargetState = state.IsName(targetState);

        if (inTargetState)
        {
            wasInState = true;

            if (state.normalizedTime >= attachTime && !hasAttached)
            {
                objectToAttach.transform.SetParent(handTarget);
                objectToAttach.transform.localPosition = positionOffset;
                objectToAttach.transform.localRotation = Quaternion.Euler(rotationOffset);
                hasAttached = true;
            }
        }
        else if (wasInState)
        {
            // Detach
            objectToAttach.transform.SetParent(null);

            if (destroyOnDetach)
            {
                Destroy(objectToAttach);
            }
            else if (moveToOnDetach != null)
            {
                objectToAttach.transform.position = moveToOnDetach.position;
                objectToAttach.transform.rotation = moveToOnDetach.rotation;
            }

            hasAttached = false;
            wasInState = false;
        }
    }
}
