using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField]
    private int inventorySize;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;
    public UnityAction<InventorySlot> OnInventorySlotChanged;
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for(int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }
    public bool AddToInventory(ItemScript item,int amount)
    {
        if (ContainsItem(item, out List<InventorySlot> invSlot))// Check if Item exist in inventory
        {
            foreach(var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }  
        }
        if (HasFreeSlot(out InventorySlot freeSlot))// Get the first available slot
        {
            freeSlot.UpdateInventorySlot(item, amount);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        return false;
    }

    public bool ContainsItem(ItemScript item, out List<InventorySlot> invSlot)
    {
        invSlot = inventorySlots.Where(i => i.ItemData == item).ToList();
        Debug.Log(invSlot.Count);
        return invSlot.Count > 1 ? true:false;
    }
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
