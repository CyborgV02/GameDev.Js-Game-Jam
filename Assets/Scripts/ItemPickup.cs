using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public InventoryItem item;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))return;

        bool picked=Inventory.instance.AddItem(item);

        if (picked)
        {
            Destroy(gameObject);
        }
    }
}
