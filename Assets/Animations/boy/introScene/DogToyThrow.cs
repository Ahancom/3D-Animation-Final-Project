using UnityEngine;

public class DogToyThrow : MonoBehaviour
{
    [SerializeField] private Transform pickupHand;
    [SerializeField] private Transform sceneTransform;

    [SerializeField] private Vector3 posOffset;

    [SerializeField] private Quaternion rotOffset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        this.transform.parent = pickupHand;
        this.transform.localPosition = posOffset;
        this.transform.rotation = rotOffset;
    }

    public void Drop()
    {
        this.transform.parent = sceneTransform;
    }
}
