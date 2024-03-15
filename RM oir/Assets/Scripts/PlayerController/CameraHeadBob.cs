using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    [Header("References")]
    public Vector3 restPosition;
    public Transform cameraPosition;

    [Header("Settings")]
    public float bobSpeed = 4.8f;
    public float bobAmount = 0.05f;

    private float timer = Mathf.PI / 2;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            timer += bobSpeed * Time.deltaTime;

            Vector3 newPosition = new Vector3(Mathf.Cos(timer) * bobAmount,
                restPosition.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)), restPosition.z);
            cameraPosition.localPosition = newPosition;
        }
        else
        {
            timer = Mathf.PI / 2;
        }

        if (timer > Mathf.PI * 2)
        {
            timer = 0;
        }
    }
}
