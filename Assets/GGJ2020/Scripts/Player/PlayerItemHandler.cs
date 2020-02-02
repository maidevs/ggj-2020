using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask itemLayer;

    private PlayerInputHandler inputHandler;

    private ItemPickupSpot itemPickupSpotInRange;

    private float interactionTime = 0f;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (inputHandler.GetLeftStickAxis().magnitude > 0f)
            return;

        if(itemPickupSpotInRange != null && inputHandler.GetInteractInput() && !itemPickupSpotInRange.HasActiveItemEffect)
        {
            interactionTime += Time.deltaTime;

            itemPickupSpotInRange.TriggerPickupProcess(interactionTime, transform);
        }
        else 
        {
            interactionTime = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((itemLayer.value & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log("Item in Range");
            itemPickupSpotInRange = other.GetComponent<ItemPickupSpot>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((itemLayer.value & 1 << other.gameObject.layer) != 0)
        {
            itemPickupSpotInRange = null;
        }
    }
}
