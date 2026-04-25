using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   public static Inventory instance;
   public List<InventoryItem> items = new List<InventoryItem>();
   public int maxSlots=8;

    void Awake()
    {
        if (instance == null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory Full !");
            return false;
        }

        items.Add(item);
        return true;
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= maxSlots)
        {
            return;
        }
        InventoryItem item = items[index];
        if (item.type == InventoryItem.ItemType.Consumable)
        {
            RemoveItem(item);
            Debug.Log($"Used {item.itemName}. Healed some HP."); // didn't make the player class yet
            //still need to heal the player
        }
        else {Debug.Log($"{item.itemName}:{item.itemdescription}");}
    }


}
