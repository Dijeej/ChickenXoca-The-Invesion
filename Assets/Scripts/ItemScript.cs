using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
     public ItemType  type;
    public ActionType action;
    public Vector3Int range = new Vector3Int(5, 2, 4);
    [Header("Only UI")]
    public bool stackable = true;
    [Header("Both")]
    public Sprite image;
}

public enum ItemType {
    MeeleWeapon,
    RangedWeapon,
    Tool
}

public enum ActionType {
    fight,
    blind
}
