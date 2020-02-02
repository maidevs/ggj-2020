using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AItemEffect : MonoBehaviour
{
    [SerializeField]
    private float itemEffectDuration;
    private float effectTimer;

    [SerializeField]
    private GameObject pickupVFX;
    [SerializeField]
    private GameObject explosionVFX;
    [SerializeField]
    private float rotationSpeed;

    private GameObject pickupFX;
    private GameObject explosionFX;

    protected Transform player;
    private MeshRenderer renderer;

    public abstract void ApplyItemEffect();
    public abstract void RemoveItemEffect();

    private bool isItemActive;

    public event System.Action ItemEffectFinished;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void TriggerItem(Transform player)
    {
        Debug.Log("Effect Triggered");

        pickupFX = Instantiate(pickupVFX, transform.position, transform.rotation, null);
        explosionFX = Instantiate(explosionVFX, transform.position, transform.rotation, null);

        DestroyVFX(2f);

        renderer.enabled = false;

        isItemActive = true;

        this.player = player;

        ApplyItemEffect();
    }

    private void Update()
    {
        if (isItemActive)
            effectTimer += Time.deltaTime;

        if (effectTimer > itemEffectDuration)
        {
            RemoveItemEffect();
            OnItemEffectFinished();
        }
    }

    private void DestroyVFX(float time)
    {
        Destroy(pickupFX.gameObject, time);
        Destroy(explosionVFX.gameObject, time);
    }

    private void FixedUpdate()
    {
        transform.localEulerAngles += Vector3.up * rotationSpeed;
    }

    private void OnItemEffectFinished()
    {
        if (ItemEffectFinished != null)
            ItemEffectFinished();
    }
}
