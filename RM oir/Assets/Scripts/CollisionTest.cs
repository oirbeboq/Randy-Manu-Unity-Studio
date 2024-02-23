using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
 
    void Start()
    {
        Debug.Log("Start");
    }

    private void Update()
    {
        Debug.Log("Test");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "sliderr")
        {
            Debug.Log("This is a Slider");
        }
    }
}
