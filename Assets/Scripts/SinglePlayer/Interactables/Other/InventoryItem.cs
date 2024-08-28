using System;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public SO_InventoryItem inventoryItem;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void AddToInventory()
    {
        if (_playerInventory.GetInventory(inventoryItem.inventoryType).Contains(inventoryItem)) return;

        _playerInventory.AddItem(inventoryItem.inventoryType, inventoryItem);
        Debug.Log("Added item: " + inventoryItem.itemName + " with ID: " + inventoryItem.itemId + " to inventory: " + inventoryItem.inventoryType);
    }

    public void RemoveFromInventory()
    {
        if (!_playerInventory.GetInventory(inventoryItem.inventoryType).Contains(inventoryItem)) return;
        
        _playerInventory.RemoveItem(inventoryItem.inventoryType, inventoryItem);
        Debug.Log("Removed item: " + inventoryItem.itemName + " with ID: " + inventoryItem.itemId + " from inventory: " + inventoryItem.inventoryType);
    }
}
