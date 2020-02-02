using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 movementDirection;
    public float speed;
    public float lifetime;
    public LayerMask mask;
    public ParticleSystem impactEffect;

    private float lifetimeCounter = 0f;

    private Rigidbody rigidbody;
    private PlayerController owner;


    public void Initialize(PlayerController player) {
        this.owner = player;
    }

    public void SetMass(float mass)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = mass;

        rigidbody.AddForce(movementDirection * speed, ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null && player != owner)
            player.SetStun();

        Destroy();
    }

    public void SetColor(Color color) {
        ParticleSystem.MainModule main;

        foreach(ParticleSystem system in impactEffect.gameObject.GetComponentsInChildren<ParticleSystem>(true)) {
            main = system.main;

            main.startColor = color * 2;
        }
    }

    private void Destroy() {
        Destroy(Instantiate(impactEffect, transform.position, Quaternion.identity), 1f);
        Destroy(gameObject);
    }
}
