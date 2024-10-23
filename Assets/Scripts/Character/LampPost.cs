using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectableObjects : MonoBehaviour
{
    public GameObject breakableObject;
    public GameObject breakedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        breakedObject = Instantiate(breakedObject);
        Destroy(breakableObject);
        breakedObject.SetActive(true);
    }
}
