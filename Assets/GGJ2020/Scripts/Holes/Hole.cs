using UnityEngine;

public class Hole : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public LayerMask mask;
    public Room parentRoom;

    public Renderer renderer;
    public ParticleSystem particleSystem;

    private float maxEmission;

    public void Initialize(float maxHealth, Room parentRoom) {
        MaxHealth = maxHealth;
        Health = MaxHealth;

        this.parentRoom = parentRoom;

        maxEmission = particleSystem.emission.rateOverTime.constant;
    }

    private void OnTriggerEnter(Collider other) {
        if((mask.value & 1 << other.gameObject.layer) != 0) {
            Health -= GameController.bulletDamage;

            UpdateEmission();
        }


        if(Health <= 0) {
            parentRoom.WarnDeadHole(this);
            Destroy(gameObject);

        }
    }

    public void UpdateEmission() {
        float percentage = Health / MaxHealth;

        var emission = particleSystem.emission;
        emission.rateOverTime = maxEmission * percentage;
    }

    public Bounds GetBonds() {
        return renderer.bounds;
    }

    public static GameController GameController { get { return GameController.Instance;  } }
}
