using System.Collections;
using UnityEngine;

public class Knockback: MonoBehaviour
{
    public float radius;
    public float force;

    private void Update()
    {
        Vector3 effectorPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(effectorPos, radius);  

        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force * Time.deltaTime, effectorPos, radius);
            }
        }
    }
}