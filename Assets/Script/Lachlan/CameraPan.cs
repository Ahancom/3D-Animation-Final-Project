using UnityEngine;

public class CutsceneCameraPan : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string stateToWaitFor = "Search";
    [SerializeField] private string triggerToFire = "Thinking";

    [Header("Camera Control")]
    [SerializeField] private Transform cameraToRotate;
    [SerializeField] private Vector3 lookUpEuler = new Vector3(70, 0, 0);
    [SerializeField] private float rotateDuration = 1.5f;
    [SerializeField] private float holdDuration = 2f;

    [Header("GameObject Changes After Look Up")]
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private GameObject[] objectsToEnable;

    private Quaternion originalRotation;
    private Quaternion lookUpRotation;
    private float timer = 0f;
    private bool triggerSent = false;

    private enum CameraState { Idle, RotatingUp, Holding, RotatingDown, Done }
    private CameraState camState = CameraState.Idle;

    void Update()
    {
        if (!animator || !cameraToRotate) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (camState == CameraState.Idle && state.IsName(stateToWaitFor) && state.normalizedTime >= 1f)
        {
            originalRotation = cameraToRotate.rotation;
            lookUpRotation = Quaternion.Euler(lookUpEuler);
            camState = CameraState.RotatingUp;
            timer = 0f;
        }

        switch (camState)
        {
            case CameraState.RotatingUp:
                timer += Time.deltaTime;
                float tUp = Mathf.Clamp01(timer / rotateDuration);
                cameraToRotate.rotation = Quaternion.Slerp(originalRotation, lookUpRotation, tUp);
                if (tUp >= 1f)
                {
                    foreach (GameObject obj in objectsToDisable) if (obj) obj.SetActive(false);
                    foreach (GameObject obj in objectsToEnable) if (obj) obj.SetActive(true);

                    camState = CameraState.Holding;
                    timer = 0f;
                }
                break;

            case CameraState.Holding:
                timer += Time.deltaTime;
                if (timer >= holdDuration)
                {
                    if (!triggerSent)
                    {
                        animator.SetTrigger(triggerToFire);
                        triggerSent = true;
                    }

                    camState = CameraState.RotatingDown;
                    timer = 0f;
                }
                break;

            case CameraState.RotatingDown:
                timer += Time.deltaTime;
                float tDown = Mathf.Clamp01(timer / rotateDuration);
                cameraToRotate.rotation = Quaternion.Slerp(lookUpRotation, originalRotation, tDown);
                if (tDown >= 1f)
                {
                    camState = CameraState.Done;
                }
                break;
        }
    }
}
