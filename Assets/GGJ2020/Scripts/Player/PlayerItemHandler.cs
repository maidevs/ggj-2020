using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask itemLayer;

    private PlayerUI playerUI;
    private PlayerController playerController;

    private PlayerInputHandler inputHandler;

    private ItemPickupSpot itemPickupSpotInRange;

    private float interactionTime = 0f;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerUI = GetComponent<PlayerUI>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (inputHandler.GetLeftStickAxis().magnitude > 0f)
            return;

        UpdateUIInteractionLabel();

        if (itemPickupSpotInRange != null)
            playerUI.UpdateInteractionFillCircle(Mathf.InverseLerp(0f, itemPickupSpotInRange.pickupTime, interactionTime));

        if(playerController.IsStunned) {
            interactionTime = 0;
            return;
        }

        if (itemPickupSpotInRange != null && !playerController.IsStunned && inputHandler.GetInteractInput() && !itemPickupSpotInRange.HasActiveItemEffect)
        {
            interactionTime += Time.deltaTime;
            itemPickupSpotInRange.TriggerPickupProcess(interactionTime, transform);


            playerController.SetAnimatorTrigger("collect");
        }
        else
        {
            interactionTime = 0f;
        }
    }

    private void UpdateUIInteractionLabel()
    {
        if (itemPickupSpotInRange != null && itemPickupSpotInRange.CurrentItem != null && !itemPickupSpotInRange.HasActiveItemEffect)
            playerUI.ActivateItemInteractUI();
        else
            playerUI.DeactivateItemInteractUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((itemLayer.value & 1 << other.gameObject.layer) != 0)
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
