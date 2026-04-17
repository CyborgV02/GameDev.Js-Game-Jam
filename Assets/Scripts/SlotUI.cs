using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class SlotUI : MonoBehaviour
{
   public TextMeshProUGUI nameText;
   public void SetItem(InventoryItem item )
    {
        
        nameText.text=item.itemName;
    }
    public void SetEmpty()
    {
        
        nameText.text="------";
    }
}
