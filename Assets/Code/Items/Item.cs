using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum ItemType {
    Weapon,
    Spell,
    Armor,
    Consumable,
    Shield
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
    public int magic;
    public int speed;

    //Use Stats
    public int attackPower;
    public int healing;
    public int magicPower;

    public ItemStatistics(int attack, int attackPower, int defense, int health, int magic, int speed, int healing, int magicPower) {
        this.attack = attack;
        this.attackPower = attackPower;
        this.defense = defense;
        this.health = health;
        this.magic = magic;
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

    public readonly  int Magic() {
        return magic;
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
        return $"Attack: {attack}, Attack Power: {attackPower}, Defense: {defense}, Health: {health}, Magic: {magic}, Speed: {speed}, Magic Power: {magicPower}";
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Item/Item")]
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
    public int speed;
    public int magic;

    [Header("Use Statistics")]
    public int attackPower;
    public int healing;
    public float manaRestore;
    public int magicPower;
    public int manaCost;
    public string onUseText;

    
    public bool stackable = true;
    public Sprite image;
    public int maxStack;
    public bool consumable;
    public Vector2Int range = new Vector2Int(0, 6);

    [Header("Minigame")]

    public OpenMinigame minigameOpener;
    public GameObject minigame;
    public PlaySpellAnimation spellAnimationPrefab;

    [Header("Prefabs and Materials")]
    public Material material;
    public GameObject itemPrefab;


    public virtual string getItemDescription() {
        return itemDescription;
    }
}