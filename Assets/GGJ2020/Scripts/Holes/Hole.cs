using UnityEngine;

public class Hole : MonoBehaviour
{
    public float Health;
    public Room parentRoom;

    public Renderer renderer;

    void Initialize(float maxHealth, Room parentRoom) {
        Health = maxHealth;
        this.parentRoom = parentRoom;
    }

    private void OnCollisionEnter(Collision collision) {
        //Handle pau mole shot
    }

    public Bounds GetBonds() {
        return renderer.bounds;
    }
}
