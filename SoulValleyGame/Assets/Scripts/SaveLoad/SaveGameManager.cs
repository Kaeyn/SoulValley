using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class SaveGameManager : MonoBehaviourPunCallbacks
{
    public static SaveData data;
    [SerializeField] Button btnSave, btnLoad, btnExit;
    bool isEscape = false;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
        if(!PhotonNetwork.IsMasterClient){
            btnSave.gameObject.SetActive(false);
            // btnLoad.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        btnSave.gameObject.SetActive(false);
        btnLoad.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            if(isEscape){
                isEscape = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                btnSave.gameObject.SetActive(false);
                btnLoad.gameObject.SetActive(false);
                btnExit.gameObject.SetActive(false);
            }else{
                isEscape = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                btnSave.gameObject.SetActive(true);
                btnLoad.gameObject.SetActive(true);
                btnExit.gameObject.SetActive(true);
            }
        }
    }
    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        var saveData = data;
        SaveLoad.Save(saveData);
    }

    public static void LoadData(SaveData _data)
    {
        if(!PhotonNetwork.IsMasterClient) return;
        data = _data;
    }
    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
