using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "InventoryItem", order = 0)]
public class InventoryItem : ScriptableObject {

    public string itemName;
    public string itemdescription;
    public Sprite icon;
    public int healamount;
    public ItemType type;

    public enum ItemType {Consumable,KeyItem,Module}

    public void Use(Character target)
    {
        switch (type)
        {
            case ItemType.Consumable:
                target.Heal(healamount);
                break;
            case ItemType.KeyItem:
                // Implement key item logic here
                break;
            case ItemType.Module:
                // Implement module logic here
                break;
        }
    }

}

