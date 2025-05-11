using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private GameObject cam2;
    [SerializeField] private GameObject cam3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cam1()
    {
        this.GetComponent<Camera>().enabled = true;
        cam2.GetComponent<Camera>().enabled = false;
        cam3.GetComponent<Camera>().enabled = false;
    }

    public void Cam2()
    {
        this.GetComponent<Camera>().enabled = false;
        cam2.GetComponent<Camera>().enabled = true;
        cam3.GetComponent<Camera>().enabled = false;
    }
    
    public void Cam3()
    {
        this.GetComponent<Camera>().enabled = false;
        cam2.GetComponent<Camera>().enabled = false;
        cam3.GetComponent<Camera>().enabled = true;
    }
}
