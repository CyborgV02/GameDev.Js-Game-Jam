using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System;

public class SlotUI : MonoBehaviour
{
   public TextMeshProUGUI nameText;
   public void SetItem(InventoryItem item )
    {
        
        nameText.text=item.itemName;
        nameText.color = Color.white; 
    }
    public void SetEmpty()
    {

        nameText.text="------";
        nameText.color = Color.white;
    }

    internal void SetHighlight(bool highlighted)
    {
          Debug.Log($"{nameText.text} highlight: {highlighted}");
        nameText.color=highlighted ? Color.yellow: Color.white;
    }
}
