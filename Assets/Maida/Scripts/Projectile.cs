using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 movementDirection;
    public float speed;
    public float lifetime;
    public LayerMask mask;
    public GameObject impactEffect;

    private float lifetimeCounter = 0f;

    private Rigidbody rigidbody;
    
    public void SetMass(float mass)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = mass;

        rigidbody.AddForce(movementDirection * speed, ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if((mask.value & 1 << other.gameObject.layer) != 0)
        {
            Destroy(Instantiate(impactEffect, transform.position, Quaternion.identity),1f);
            Destroy(this.gameObject);
        }
    }
}
