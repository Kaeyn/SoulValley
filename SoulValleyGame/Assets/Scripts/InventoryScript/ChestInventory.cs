using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    /*public UnityAction<IInteractable> OnInteractableComplete { get; set; }*/

    [SerializeField] private ItemScript itemData;
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem,transform.position,transform.rotation, itemData);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        // check the save data for specific chest inventory - if exist load in
        if(data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    public void Interact(Interactor interactor)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0);
        InventoryUIControler.isClosed = false;
    }
    /*public void EndInteraction()
    {
       
    }*/

}


