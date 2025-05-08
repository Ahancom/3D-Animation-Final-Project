using UnityEngine;

public class Raiser : MonoBehaviour
{
    [SerializeField] private Transform object1;
    [SerializeField] private Transform object2;
    [SerializeField] private float targetHeight = 3f;
    [SerializeField] private float raiseSpeed = 2f;
    [SerializeField] private string triggeringTag = "Player";

    private bool shouldRaise = false;
    private Vector3 object1StartPos;
    private Vector3 object2StartPos;
    private Vector3 object1TargetPos;
    private Vector3 object2TargetPos;

    private void Start()
    {
        object1StartPos = object1.position;
        object2StartPos = object2.position;

        object1TargetPos = new Vector3(object1StartPos.x, targetHeight, object1StartPos.z);
        object2TargetPos = new Vector3(object2StartPos.x, targetHeight, object2StartPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
            shouldRaise = true;
    }

    private void Update()
    {
        if (shouldRaise)
        {
            object1.position = Vector3.MoveTowards(object1.position, object1TargetPos, raiseSpeed * Time.deltaTime);
            object2.position = Vector3.MoveTowards(object2.position, object2TargetPos, raiseSpeed * Time.deltaTime);
        }
    }
}
