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

public struct ItemStatistics {
    public int attack;
    public int defense;
    public int health;
    public int mana;
    public int speed;

    public ItemStatistics(int attack, int defense, int health, int mana, int speed) {
        this.attack = attack;
        this.defense = defense;
        this.health = health;
        this.mana = mana;
        this.speed = speed;
    }

    public readonly  int Attack() {
        return attack;
    }

    public readonly  int Defense() {
        return defense;
    }

    public readonly  int Health() {
        return health;
    }

    public readonly  int Mana() {
        return mana;
    }

    public readonly  int Speed() {
        return speed;
    }

    public readonly override string ToString() {
        return $"Attack: {attack}, Defense: {defense}, Health: {health}, Mana: {mana}, Speed: {speed}";
    }
}

[CreateAssetMenu(menuName = "Scriptable Item")]
public class Item : ScriptableObject {
    [Header("Gameplay")]

    public ItemType type;
    public ActionType actionType;
    
    [Header("Item Statistics")]
    public int attack;
    public int defense;
    public int health;
    public int mana;
    public int speed;

    [Header("UI")]
    public Sprite image;
    public bool stackable = true;
    public int maxStack;
    public Vector2Int range = new Vector2Int(0, 5);
}