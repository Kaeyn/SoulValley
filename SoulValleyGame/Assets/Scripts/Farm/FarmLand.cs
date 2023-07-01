using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour, ITimeTracker
{
    public GameObject select;
    [Header("Crop")]
    public GameObject cropPrefab;
    public CropBehaviour cropPlanted = null;
    public enum LandStatus
    {
        Dry, 
        Watered
    }
    public LandStatus landStatus = LandStatus.Dry;
    GameTimeStamp timeWatered;
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    public bool Plant(SeedData seed)
    {
        if(cropPlanted == null)
        {
            Debug.Log(transform.position);
            GameObject cropObject = Instantiate(cropPrefab,transform);
            cropObject.transform.localPosition = new Vector3(0, 0, 0);
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.PLant(seed);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Water()
    {
        landStatus = LandStatus.Watered;
        timeWatered = TimeManager.Instance.GetTimeStamp();

    }
    public void Harvest(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;
        List<Transform> children = new List<Transform>();
        foreach(Transform child in cropPlanted.transform)
        {
            children.Add(child);
        }
        if (inventory.AddToInventory(children[children.Count - 1].GetComponent<ItemPickUp>().itemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(children[children.Count  - 1].GetComponent<UniqueID>().ID);
            Destroy(cropPlanted.gameObject);
            cropPlanted = null;
        }
    }
    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if(landStatus == LandStatus.Watered)
        {
            cropPlanted.Grow();
            if(GameTimeStamp.CompareTimeStamp(timeWatered, timeStamp, false) >= 24 * 60)
            {
                landStatus = LandStatus.Dry;
            }
        }
    }
}
