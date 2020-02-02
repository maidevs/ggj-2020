using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupSpot : MonoBehaviour
{
    [SerializeField]
    private float pickupTime;

    [SerializeField]
    private float itemRespawnTime;
    private float respawnTimer;

    [SerializeField]
    private List<AItemEffect> items;
    
    public AItemEffect CurrentItem { get; private set; }

    public event Action ItemSpawned;

    public bool HasActiveItemEffect;

    void Start()
    {
       
    }

    [NaughtyAttributes.Button("SpawnItem")]
    private void SpawnItem()
    {
        respawnTimer = 0f;

        UIController.Instance.WarnAboutItemSpawn();

        var randomItem = items[UnityEngine.Random.Range(0, items.Count)];

        CurrentItem = Instantiate(randomItem, transform.position, randomItem.transform.rotation).GetComponent<AItemEffect>();
    }
    
    public void TriggerPickupProcess(float interactionTimer, Transform player)
    {
        if (interactionTimer >= pickupTime && CurrentItem != null && !HasActiveItemEffect)
        {
            CurrentItem.ItemEffectFinished += OnItemEffectFinished;
            HasActiveItemEffect = true;
            CurrentItem.TriggerItem(player);
            respawnTimer = 0f;
        }
    }
    
    private void OnItemSpawned()
    {
        if (ItemSpawned != null)
            ItemSpawned();
    }

    private void OnItemEffectFinished()
    {
        HasActiveItemEffect = false;
        CurrentItem.ItemEffectFinished -= OnItemEffectFinished;

        Destroy(CurrentItem.gameObject);
        CurrentItem = null;
    }

    void Update()
    {
        if (CurrentItem == null)
            respawnTimer += Time.deltaTime;

        if (CurrentItem == null && respawnTimer > itemRespawnTime)
            SpawnItem();
    }
}
