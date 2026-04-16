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

}

