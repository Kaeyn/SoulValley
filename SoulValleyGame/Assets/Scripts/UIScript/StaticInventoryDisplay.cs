﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;
    public InventorySlot InventorySlot;

    int selectedSlot=-1;
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
        ChangedSelectedSlot(0);
        
    }
    private void throwItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;
        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            if (isShiftPress){
                selectedSlotData.ClearSlot();
            }
            else
            {
                if (selectedSlotData.StackSize > 1)
                {
                    selectedSlotData.RemoveFromStack(1);
                    selectedUISlot.UpdateUISlot(selectedSlotData);
                    Debug.Log("Throw Item: " + itemData.DisplayName + " - " + selectedSlotData.StackSize);
                }
                else
                {
                    selectedSlotData.ClearSlot();
                }
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
            selectedUISlot.UpdateUISlot(selectedSlotData);
            Debug.Log("Throw Item: " + itemData.DisplayName);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    private void UseItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;

        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            if (itemData.MaxStackSize > 1)
            {
                selectedSlotData.RemoveFromStack(1);
                selectedUISlot.UpdateUISlot(selectedSlotData);
                if (selectedSlotData.StackSize < 1)
                {
                    selectedSlotData.ClearSlot();
                    selectedUISlot.UpdateUISlot(selectedSlotData);
                }
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
            Debug.Log("USe Item: " + itemData.DisplayName);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    private bool GetSelectedItem(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        InventorySlot slot = inventorySystem.GetSlot(selectedSlot);

        if (slot != null && slot.ItemData != null)
        {
            string itemName = slot.ItemData.DisplayName;
            int itemCount = slot.StackSize;

            // Sử dụng thông tin của item
            Debug.Log("Item Name: " + itemName);
            Debug.Log("Item Count: " + itemCount);
            return true;
        }
        else
        {
            Debug.Log("No item in the specified slot.");
            return false;
        }
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q) && GetSelectedItem(selectedSlot))
        {
            throwItem(selectedSlot);
        }
        if(Mouse.current.leftButton.wasPressedThisFrame && GetSelectedItem(selectedSlot))
        {
            UseItem(selectedSlot);
        }

        float scrollValue = Input.mouseScrollDelta.y;

        if (selectedSlot > -1 && selectedSlot < 9)
        {
            if (scrollValue < 0 && selectedSlot < 8)
            {
                //Debug.Log("cuon len");
                ChangedSelectedSlot(selectedSlot + 1);
                GetSelectedItem(selectedSlot);
            }
            else if (scrollValue > 0 && selectedSlot > 0)
            {
                //Debug.Log("cuon xuong");
                ChangedSelectedSlot(selectedSlot - 1);
                GetSelectedItem(selectedSlot);
            }
        }
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number >0 && number< 10)
            {
                //Debug.Log("Phim "+number);
                ChangedSelectedSlot(number - 1);
                GetSelectedItem(selectedSlot);
            }
        }

    }

    private void ChangedSelectedSlot(int value)
    {
        if (selectedSlot >= 0)
        {
            slots[selectedSlot].Deselected();
        }
        slots[value].Selected();
        selectedSlot = value;
    }
  
    public void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSLot;
        }
        else
        {
            Debug.Log("No inventory assign to: " + this.gameObject);
        }
        AssignSlot(inventorySystem);
    }
    public override void AssignSlot(InventorySystem invDisplay) 
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();
        if (slots.Length != inventorySystem.InventorySize) Debug.Log("Inventory slot out of on " + this.gameObject);
        for(int i =0; i< inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
   

}
