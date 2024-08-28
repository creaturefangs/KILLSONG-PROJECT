using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItem", order = 1)]
public class SO_InventoryItem : ScriptableObject
{
    public string itemName;
    public int itemId;
    public PlayerInventory.InventoryTypes inventoryType;
}
