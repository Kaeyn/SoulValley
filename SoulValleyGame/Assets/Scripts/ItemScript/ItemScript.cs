using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// A scriptable object, define what an item in the game
// 

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class ItemScript : ScriptableObject
{
    public int Id;
    public string DisplayName;

    [Header("Only gameplay")]
    [TextArea(4, 4)]
    public string Description;
    public int MaxStackSize;
    public Sprite icon;
    public GameObject ItemPreFab;

  

}
