using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUIControler : MonoBehaviour
{
    public static bool isClosed = true;
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;
    public Image backGround;


    internal static bool status = false;
    private void Start()
    {
        playerBackpackPanel.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        backGround.enabled=false;

    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
        
    }
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClosed)
        {
            close();
        }
    }
    public void close()
    {
        if (chestPanel.gameObject.activeInHierarchy)
        {

            chestPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerBackpackPanel.gameObject.activeInHierarchy)
        {
            Debug.Log("hien");
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void DisplayInventory(InventorySystem invDisplay)
    {
        chestPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invDisplay);
        playerBackpackPanel.gameObject.SetActive(true);
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
       
    }
    void DisplayPlayerBackpack(InventorySystem invDisplay)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplay);
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    
}
