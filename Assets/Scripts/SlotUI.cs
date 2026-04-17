using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class SlotUI : MonoBehaviour
{
   public Image iconImage;
   public TextMeshProUGUI nameText;
   public void SetItem(InventoryItem item )
    {
        iconImage.sprite=item.icon;
        iconImage.enabled=true;
        nameText.text=item.itemName;
    }
    public void SetEmpty()
    {
        iconImage.enabled=false;
        nameText.text="------";
    }
}
