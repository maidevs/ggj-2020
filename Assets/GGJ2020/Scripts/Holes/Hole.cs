using UnityEngine;

public class Hole : MonoBehaviour
{
    public float Health;
    public LayerMask mask;
    public Room parentRoom;

    public Renderer renderer;

    public void Initialize(float maxHealth, Room parentRoom) {
        Health = maxHealth;
        this.parentRoom = parentRoom;
    }

    private void OnTriggerEnter(Collider other) {
        if((mask.value & 1 << other.gameObject.layer) != 0) {
            Health -= GameController.bulletDamage;
        }

        if(Health <= 0) {
            parentRoom.WarnDeadHole(this);
            Destroy(gameObject);
        }
    }

    public Bounds GetBonds() {
        return renderer.bounds;
    }

    public static GameController GameController { get { return GameController.Instance;  } }
}
