using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum InventoryTypes
    {
        Normal,
        Hidden
    }

    private Dictionary<InventoryTypes, List<SO_InventoryItem>> _inventoryItems;

    public PlayerInventory()
    {
        _inventoryItems = new Dictionary<InventoryTypes, List<SO_InventoryItem>>
        {
            { InventoryTypes.Normal, new List<SO_InventoryItem>() },
            { InventoryTypes.Hidden, new List<SO_InventoryItem>() }
        };
    }

    public List<SO_InventoryItem> GetInventory(InventoryTypes type)
    {
        return _inventoryItems[type];
    }

    public void AddItem(InventoryTypes type, SO_InventoryItem item)
    {
        _inventoryItems[type].Add(item);
    }

    public void RemoveItem(InventoryTypes type, SO_InventoryItem item)
    {
        _inventoryItems[type].Remove(item);
    }

    public bool GetInventoryItemById(InventoryTypes type, int itemID)
    {
        if (_inventoryItems.TryGetValue(type, out var inventoryItem))
        {
            foreach (SO_InventoryItem item in inventoryItem)
            {
                if (item.itemId == itemID)
                {
                    //item found
                    return true;
                }
            }
        }
        //item not found
        return false;
    }

}