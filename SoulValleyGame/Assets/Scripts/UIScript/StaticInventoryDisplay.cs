﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;
    public InventorySlot InventorySlot;
    public int selectedSlot=-1;
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
        ChangedSelectedSlot(0);
    }
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }
    public void throwItem(Transform transform,int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;
        Vector3 positionToSpawn = transform.position + transform.forward * 1f;
        if (selectedSlotData != null && selectedSlotData.ItemData != null){
            GameObject itemGameObject = selectedSlotData.ItemData.ItemPreFab;
            if (isShiftPress){
                for(int i = 0; i < selectedSlotData.StackSize; i++)
                {
                    var position = new Vector3(Random.Range(-0.3f, -0.1f), 0, Random.Range(-0.3f, -0.1f));
                    Vector3 _dropOffset = position;
                    Instantiate(itemGameObject, positionToSpawn + _dropOffset, Quaternion.identity);  
                }
                selectedSlotData.ClearSlot();    
            }
            else
            {
                if (selectedSlotData.GetCurrentStackSize() > 1)
                {
                    selectedSlotData.RemoveFromStack(1);
                    selectedUISlot.UpdateUISlot(selectedSlotData);
                    Instantiate(itemGameObject, positionToSpawn, Quaternion.identity);
                }
                else
                {
                    selectedSlotData.ClearSlot();
                    Instantiate(itemGameObject, positionToSpawn, Quaternion.identity);
                }
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    public void UseItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            if (selectedSlotData.GetCurrentStackSize() > 1)
            {
                selectedSlotData.RemoveFromStack(1);
            }
            else
            {
                selectedSlotData.ClearSlot();
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
    }
    public bool GetSelectedItem(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        InventorySlot slot = inventorySystem.GetSlot(selectedSlot);
        if (slot != null && slot.ItemData != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public SeedData GetSeed(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        SeedData slot = inventorySystem.GetSlot(selectedSlot).ItemData as SeedData;
        return slot;
    }
    public PlaceableData GetPlaceableData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        PlaceableData data = inventorySystem.GetSlot(selectedSlot).ItemData as PlaceableData;
        return data;
    }
    public ToolData GetToolData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        ToolData data = inventorySystem.GetSlot(selectedSlot).ItemData as ToolData;
        return data;
    }
    public void ChangedSelectedSlot(int value)
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
           // Debug.Log("No inventory assign to: " + this.gameObject);
        }
        AssignSlot(inventorySystem,0);
    }
    public override void AssignSlot(InventorySystem invDisplay,int offset) 
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for(int i = 0; i < inventoryHolder.Offset; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
    private void Update() {
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
}
