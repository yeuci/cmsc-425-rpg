using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum ItemType {
    Weapon,
    Armor,
    Consumable
}

public enum ActionType {
    Attack,
    Defend,
    Consume
}

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject {
    [Header("Only gameplay")]

    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);


    [Header("UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}