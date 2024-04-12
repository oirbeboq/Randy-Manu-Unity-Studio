using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandForce : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                IHitable hitable =  hit.transform.GetComponent<IHitable>();
                if (hitable != null)
                {
                    hitable.Execute(transform);
                }
            }
        }
    }
}
