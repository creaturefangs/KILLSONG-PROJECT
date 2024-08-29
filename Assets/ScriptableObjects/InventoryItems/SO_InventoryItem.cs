using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItem", order = 1)]
public class SO_InventoryItem : ScriptableObject
{
    [Tooltip("Name of the inventory item")]
    public string itemName;

    [Tooltip("ID for the inventory item")]
    public int itemId;

    [Tooltip("Type of inventory this item belongs to (e.g., Normal, Hidden)")]
    public PlayerInventory.InventoryTypes inventoryType;
}