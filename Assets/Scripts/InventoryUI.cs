using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("References")]
    public Transform slotGrid;
    public TextMeshProUGUI descriptionText;

    [Header("Settings")]
    public bool isOpen = false;
    private SlotUI[] slots;
    private int selectedIndex = 0;

    void OnDestroy()
    {
        InputController.OnActionC -= ToggleInventory;
        InputController.OnMove -= HandleNavigate;
        InputController.OnActionZ -= HandleConfirm;
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        slots = slotGrid.GetComponentsInChildren<SlotUI>();

        InputController.OnActionC += ToggleInventory;
        InputController.OnMove += HandleNavigate;
        InputController.OnActionZ += HandleConfirm;
    }

    void Start()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }

    private void OpenInventory()
    {
        isOpen = true;
        gameObject.SetActive(true);
        selectedIndex = 0;
        RefreshUI();
        Time.timeScale=0;
    }

    private void CloseInventory()
    {
        gameObject.SetActive(false);
        isOpen = false;
        Time.timeScale=1;
    }

    private void RefreshUI()
    {
        List<InventoryItem> items = Inventory.instance.items;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count) slots[i].SetItem(items[i]);
            else slots[i].SetEmpty();
        }
        UpdateHighlight();
        UpdateDescription();
    }

    private void UpdateHighlight()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetHighlight(i == selectedIndex);
    }

    private void UpdateDescription()
    {
        List<InventoryItem> items = Inventory.instance.items;
        if (items.Count == 0) { descriptionText.text = "No items."; return; }
        InventoryItem selected = items[selectedIndex];
        descriptionText.text = $"{selected.itemName}\n{selected.itemdescription}";
    }

    void ToggleInventory()
    {
        if (isOpen) CloseInventory();
        else OpenInventory();
    }

    public void MoveSelection(int direction)
    {
        int count = Inventory.instance.items.Count;
        if (count == 0) return;
        selectedIndex = (selectedIndex + direction + count) % count;
        UpdateHighlight();
        UpdateDescription();
    }

    public void ConfirmSelection()
    {
        Inventory.instance.UseItem(selectedIndex);
        RefreshUI();
    }

    private void HandleNavigate(Vector2 dir)
    {
        if (!isOpen) return;
        if (dir.y > 0.5f) MoveSelection(-1);
        else if (dir.y < -0.5f) MoveSelection(1);
    }

    private void HandleConfirm()
    {
        if (!isOpen) return;
        ConfirmSelection();
    }
}