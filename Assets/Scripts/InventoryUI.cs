using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
   public static InventoryUI Instance;

   [Header("References")]
   public Transform slotGrid;
   public GameObject inventoryPanel;
   public TextMeshProUGUI descriptionText;
   public RectTransform cursorArrow;

   [Header("Settings")]
   public float slotHeight=48f;
   public bool isOpen=false;
   private SlotUI[] slots;
   private int selectedIndex=0;


    void Awake()
    {
        if (Instance == null)
        {
            Instance=this;
        }
        else {Destroy(gameObject);}
    }

    void Start()
    {
        slots=slotGrid.GetComponentsInChildren<SlotUI>(); 
        inventoryPanel.SetActive(false);
    }

    private void OpenInventory()
    {
        isOpen=true;
        inventoryPanel.SetActive(true);
        selectedIndex=0;
        RefreshUI();
    }

    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        isOpen=(false);
    }

    private void RefreshUI()
    {
        List<InventoryItem> items =Inventory.instance.items;
        for (int i  =0 ;i< slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else {slots[i].SetEmpty();}
        }
         UpdateCursor();
        UpdateDescription();
    }

    private void UpdateDescription()
    {
        List<InventoryItem> items = Inventory.instance.items;
        if (items.Count == 0)
        {
            descriptionText.text = "No items.";
            return;
        }
        InventoryItem selected = items[selectedIndex];
        descriptionText.text = $"{selected.itemName}\n{selected.itemdescription}";
    }

    private void UpdateCursor()
    {
        if (Inventory.instance.items.Count == 0)
        {
            return;
        }
        Vector3 pos=slots[selectedIndex].transform.position;
        cursorArrow.position = new Vector3(cursorArrow.position.x, pos.y, 0); 
    }

    public void MoveSelection(int direction)
    {
        int count =Inventory.instance.items.Count;
        if (count==0) return;
        selectedIndex=(selectedIndex+direction+count)%count;
        UpdateCursor();
        UpdateDescription();
    }
    public void ConfirmSelection()
    {
        Inventory.instance.UseItem(selectedIndex);
        RefreshUI();
    }
    public int GetSelectedIndex() => selectedIndex;
}



