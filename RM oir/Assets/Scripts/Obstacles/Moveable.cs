using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour,IHitable
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Execute(Transform executionSource)
    {
        KnockbackEntity(executionSource);
    }

    public void KnockbackEntity(Transform executionSource)
    {
        Vector3 dir = (transform.position - executionSource.transform.position).normalized;
        rb.AddForce(dir, ForceMode.Impulse);
    }
}
