using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
[RequireComponent(typeof(Rigidbody))]
public class ItemPickUp : MonoBehaviourPunCallbacks
{
    [SerializeField] private float rotationSpeed = 20f;
    float groundRadius = .25f;
    public float PickUpRadius = 1f;
    public ItemScript itemData;

    private SphereCollider myCollider;

    PhotonView view;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    Rigidbody rb;
    private string id;
    float minPositionY;
    float maxPositionY;
    bool isMax = false;
    bool isSet = false;
    bool isGround = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(PhotonNetwork.IsMasterClient)
            SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(itemData, transform.position, transform.rotation);

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
        myCollider.center = new Vector3(0, PickUpRadius/2, 0);
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
        id = GetComponent<UniqueID>().ID;
        photonView.RPC("CallMasterOnStart",RpcTarget.MasterClient,view.ViewID);
    }

    private void LoadGame(SaveData data)
    {
        SaveLoad.OnLoadGame -= LoadGame;
        SaveGameManager.data.activeItems.Remove(id);
        view.RPC("DestroyItem", RpcTarget.AllBufferedViaServer,view.ViewID);
    }  

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if(inventory){
            if(inventory.AddToInventory(itemData,1)){
                if(PhotonNetwork.IsMasterClient)
                    SaveLoad.OnLoadGame -= LoadGame;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.itemCollected, this.transform.position);
                photonView.RPC("CallMasterOnDestroy",RpcTarget.MasterClient,view.ViewID);
                view.RPC("DestroyItem", RpcTarget.AllBufferedViaServer,view.ViewID);
            }
        }
    }

    private void Update()
    {
        if(!isGround){
            Collider[] colliders = Physics.OverlapSphere(transform.position,groundRadius, whatIsGround);
            if(colliders.Length != 0){
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                isGround = true;
            }
        }else{
            idle();
        }
        
    }
    
    void idle(){
        if(!isSet){
            minPositionY = transform.position.y;
            maxPositionY = transform.position.y + 0.1f;
            isSet = true;
        }
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (!isMax)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, maxPositionY, transform.position.z), 0.001f);
            if (transform.position.y >= maxPositionY)
            {
                isMax = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, minPositionY, transform.position.z), 0.001f);
            if (transform.position.y <= minPositionY)
            {
                isMax = false;
            }
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,groundRadius);
    }
    [PunRPC]
    private void DestroyItem(int viewID)
    {
        Destroy(gameObject);
    }
    [PunRPC]
    private void CallMasterOnDestroy(int viewID)
    {
        string id = PhotonView.Find(viewID).gameObject.GetComponent<UniqueID>().ID;
        if(SaveGameManager.data.activeItems.ContainsKey(id))
            SaveGameManager.data.activeItems.Remove(id);
    }
    [PunRPC]
    private void CallMasterOnStart(int viewID)
    {
        string id = PhotonView.Find(viewID).gameObject.GetComponent<UniqueID>().ID;
        ItemPickUpSaveData itemSaveData = PhotonView.Find(viewID).gameObject.GetComponent<ItemPickUp>().itemSaveData;
        if(SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems[id] = itemSaveData;
        }
        else 
        {
            SaveGameManager.data.activeItems.Add(id,itemSaveData);
        }
    }
}


[System.Serializable]
public struct ItemPickUpSaveData
{
    public ItemScript itemData;
    public Vector3 position;
    public Quaternion rotation;

    public ItemPickUpSaveData(ItemScript _itemData, Vector3 _position, Quaternion _rotation)
    {
        itemData = _itemData;
        position = _position;
        rotation = _rotation;
    }
}
