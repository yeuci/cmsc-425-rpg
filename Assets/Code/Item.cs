using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum ItemType {
    Weapon,
    Spell,
    Armor,
    Consumable
}

public enum ActionType {
    Attack,
    Cast,
    Defend,
    Consume
}

public struct ItemStatistics {

    //Equip Statistics
    public int attack;
    public int defense;
    public int health;
    public int mana;
    public int speed;

    //Use Stats
    public int attackPower;
    public int healing;
    public int magicPower;

    public ItemStatistics(int attack, int attackPower, int defense, int health, int mana, int speed, int healing, int magicPower) {
        this.attack = attack;
        this.attackPower = attackPower;
        this.defense = defense;
        this.health = health;
        this.mana = mana;
        this.speed = speed;
        this.healing = healing;
        this.magicPower = magicPower;
        Debug.Log(this);
    }

    public readonly  int Attack() {
        return attack;
    }

    public readonly  int AttackPower() {
        return attackPower;
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

    public readonly int Healing() {
        return healing;
    }
    public readonly int MagicPower() {
        return magicPower;
    }

    public readonly override string ToString() {
        return $"Attack: {attack}, Attack Power: {attackPower}, Defense: {defense}, Health: {health}, Mana: {mana}, Speed: {speed}";
    }
}

[CreateAssetMenu(menuName = "Scriptable Item")]
public class Item : ScriptableObject {
    new public string name = "Default name";
    public string itemDescription = "Some description";
    [Header("Gameplay")]

    public ItemType type;
    public ActionType actionType;
    
    [Header("Equip Statistics")]
    public int attack;
    public int defense;
    public int health;
    public int mana;
    public int speed;

    [Header("Use Statistics")]
    public int attackPower;
    public int healing;
    public int magicPower;
    public int manaCost;

    
    public bool stackable = true;
    public Sprite image;
    public int maxStack;
    public bool consumable;
    public Vector2Int range = new Vector2Int(0, 6);

    [Header("Minigame")]

    public OpenMinigame minigameOpener;
    public GameObject minigame;


    public virtual string getItemDescription() {
        return itemDescription;
    }
}