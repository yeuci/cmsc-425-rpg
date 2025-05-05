using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public enum ScalingMethod {
        PLAYER_LEVEL,
        CLASS,
        CUSTOM
    }

[System.Serializable]
public class Entity : MonoBehaviour
{
    [SerializeField] public Stat stats;
    [SerializeField] public ItemSave[] equippedGear;
    [SerializeField] public int equippedGearCount = 0;

    // proxy for inventory
    // [SerializeField] public List<Item> inventory = new List<Item>(new Item[25]);
    [SerializeField] public ItemSave[] inventory = new ItemSave[25];
    [SerializeField] public int inventoryCount = 0;

    [SerializeField] public Class eClass;

    // Remaining HP of entity
    public float remainingHP;
    public float remainingMP;

    // Alive check (Should destroy Entity gameobject if false)
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public List<int> defeatedEnemies = new List<int>();

    Item [] availableItems;


    // used to determine enemy gameobject for before and after combat scene. not needed for anything else
    // each anemy should be assigned a unique id in the editor
    public int enemyId = 0;

    void Start() {
        equippedGear = new ItemSave[3]; //Changed Size of equippedGear to match number of slots
        remainingHP = stats.health;
        remainingMP = stats.mana;

        availableItems = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<AvailableItemsAccess>().availableItems;
        if (availableItems != null) {
            if(eClass != Class.ENEMY) {
                AddPlayerEquipment();
            } else {
                AddEquipment();
            }
        }

    }

    // Basic Entity
    public Entity() {
        stats = new Stat();
        remainingHP = stats.health;
        remainingMP = stats.mana;
    }

    public float calculateXPValue() {
        return stats.getStatTotal();
    }

    public void recalculateLvl() {
        if (stats.experience >= stats.expToNext) {
            stats.level += 1;
            stats.experience -= stats.expToNext;
            stats.expToNext *= 2;
            recalculateLvl();
            float prevXP = stats.experience;
            scaleStats(ScalingMethod.PLAYER_LEVEL);
            stats.experience = prevXP;
        } else {
            return;
        }
    }

    public bool IsAlive() {
        if (remainingHP <= 0) {
            isAlive = false;
            return false;
        }
        return true;
    }

    public Stat getAdjustedStats() {
        //Attack, Defense, Health, Magic Speed
        float[] adjStats = this.stats.getStatArray();
        //I need to re-order this based upon the actual implementation of Stat Array
        foreach (ItemSave iS in equippedGear){
            if(iS != null && iS.itemData != null) {
                Item i = iS.itemData;
                adjStats[0] += i.health;
                adjStats[1] += i.mana;
                adjStats[2] += i.attack;
                adjStats[3] += i.defense;
                adjStats[4] += i.speed;
                adjStats[5] += i.magic;
            }
        }
        //STATS: lvl, health, mana, atk, def, spd, mgk
        Stat stats = new Stat(this.stats.level, adjStats[0],adjStats[1],adjStats[2],adjStats[3],adjStats[4], adjStats[5]);
        return stats;
    }

    public void scaleStats(ScalingMethod scaleMethod, float[] scalings = default) {
        float constantScale = 1.0f;
        if (scalings == default) constantScale = UnityEngine.Random.Range(0.8f, 1.2f);

        switch (scaleMethod) {
            case ScalingMethod.PLAYER_LEVEL: // scale stats based on player level
                stats = new Stat(PlayerManager.player.entity().stats.level, constantScale);
                break;
            case ScalingMethod.CLASS: // scale stats based on class
                // add later
                break;
            case ScalingMethod.CUSTOM: // scale stats based on player level and scalings[] array
                stats = new Stat(PlayerManager.player.entity().stats.level, scalings);
                break;
        }

        remainingHP = stats.health;
        remainingMP = stats.mana;
    }

    private void AddEquipment() {

        inventory[0] = new ItemSave(2,"Healing Potion",availableItems[1]);
        if(stats.magic > stats.attack) {
            //I will want to give them a consumable spell.
            equippedGear[2] = new ItemSave(1,"Spell",availableItems[UnityEngine.Random.Range(5,8)]);
            //No armor initially, and Unarmed Strike
        } else {
            //Give them a weapon and armor. These should be basic.
            equippedGear[0] = new ItemSave(1,"Leather Armor",availableItems[4]);
            equippedGear[1] = new ItemSave(1,"Basic Sword",availableItems[3]);
            //Do not give them a spell
        }
        //I do need to send this to state if this is the player.

    }

    //This should only run in DungeonMap, so I can access InventoryManager
    private void AddPlayerEquipment() {
        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        inventoryManager.AddItem(availableItems[1]);
        inventoryManager.AddItem(availableItems[1]);
        if(stats.magic > stats.attack) {
            inventoryManager.AddItem(availableItems[UnityEngine.Random.Range(5,8)]);
        } else {
            inventoryManager.AddItem(availableItems[3]);
            inventoryManager.AddItem(availableItems[4]);
        }
        inventoryManager.SendCurrentInventoryToState();

    }
}
