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
    [SerializeField] public ItemSave[] equippedGear = new ItemSave[3];
    [SerializeField] public int equippedGearCount = 0;

    // proxy for inventory
    // [SerializeField] public List<Item> inventory = new List<Item>(new Item[25]);
    [SerializeField] public ItemSave[] inventory = new ItemSave[25];
    [SerializeField] public int inventoryCount = 0;

    [SerializeField] public Class eClass;

    // Remaining HP of entity
    public float remainingHP, maximumHP = 0;
    public float remainingMP, maximumMP = 0;

    // Alive check (Should destroy Entity gameobject if false)
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public List<int> defeatedEnemies = new List<int>();

    public int skillPoints;

    // used to determine enemy gameobject for before and after combat scene. not needed for anything else
    // each anemy should be assigned a unique id in the editor
    public int enemyId = 0;

    void Start() {
        remainingHP = 10*stats.health;
        remainingMP = 5*stats.magic;
        maximumHP = 10*stats.health;
        maximumMP = 5*stats.magic;

        skillPoints = 0;
    }

    // Basic Entity
    public Entity() {
        stats = new Stat();
        equippedGear = new ItemSave[3];
        remainingHP = 10*stats.health;
        remainingMP = 5*stats.magic;
        maximumHP = 10*stats.health;
        maximumMP = 5*stats.magic;
    }
    
    public float calculateXPValue() {
        return (int)Mathf.Sqrt(150 * stats.getStatTotal());
    }

    public void recalculateLvl() {
        if (stats.experience >= stats.expToNext) {
            stats.level += 1;
            stats.experience -= stats.expToNext;
            stats.expToNext *= 2;
            skillPoints += 5;
            recalculateLvl();
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
        //health, attack, defense, speed, magic
        float[] adjStats = this.stats.getStatArray();
        //I need to re-order this based upon the actual implementation of Stat Array
        foreach (ItemSave iS in equippedGear){
            if(iS != null && iS.itemData != null) {
                Item i = iS.itemData;
                adjStats[0] += i.health;
                adjStats[1] += i.attack;
                adjStats[2] += i.defense;
                adjStats[3] += i.speed;
                adjStats[4] += i.magic;
                
                Debug.Log(this.name + ", " + iS.itemData.defense);
            }
        }
        //STATS: lvl, health, mana, atk, def, spd, mgk
        Stat stats = new Stat(this.stats.level, adjStats[0],adjStats[1],adjStats[2],adjStats[3],adjStats[4]);
        maximumHP = 10*stats.health;    
        maximumMP = 5*stats.magic;
        Debug.Log("Adjusted Stats for "+eClass+":\nHealth: "+stats.health+", Atk: "+stats.attack+", Def: "+stats.defense+
                        "Speed: "+stats.speed+", Mgk: "+stats.magic+"\nHP: "+remainingHP+"/"+maximumHP+
                        "\nStat Total = "+stats.getStatTotal()+"\nMP: "+remainingMP+"/"+maximumMP);
        return stats;
    }

    public void scaleStats(ScalingMethod scaleMethod, float[] scalings = default) {
        float[] constantScales = new float[5];
        if (scalings == default) {
            for(int i = 0; i < 5; i++) constantScales[i] = UnityEngine.Random.Range(0.8f, 1.2f);
        }

        switch (scaleMethod) {
            case ScalingMethod.PLAYER_LEVEL: // scale stats based on player level
                float[] newStats = stats.getStatArray();
                int level = PlayerManager.player.entity().stats.level;
                for(int i = 0; i < 5; i++) {    
                    newStats[i] += level * constantScales[i];
                }
                stats = new Stat(level, newStats[0], newStats[1], newStats[2], newStats[3], newStats[4]);
                break;
            case ScalingMethod.CLASS: // scale stats based on class
                // add later
                break;
            case ScalingMethod.CUSTOM: // scale stats based on player level and scalings[] array
                stats = new Stat(PlayerManager.player.entity().stats.level, scalings);
                break;
        }

        remainingHP = 10*stats.health;
        remainingMP = 5*stats.magic;
        maximumHP = 10*stats.health;
        maximumMP = 5*stats.magic;
    }

    public void AddEquipment(Item[] availableItems) {

        inventory[0] = new ItemSave(2,"Healing Potion",availableItems[1]);
        if(equippedGear.Length == 0) equippedGear = new ItemSave[3];

        if(stats.magic > stats.attack) {
            //I will want to give them a consumable spell.
            inventory[1] = new ItemSave(1,"Spell",availableItems[UnityEngine.Random.Range(5,8)]);
            equippedGear[0] = new ItemSave(1, "Leather Armor", availableItems[4]);
            equippedGear[1] = new ItemSave(1, "Unarmed Attack", availableItems[9]);
            //No armor initially, and Unarmed Strike
        } else {
            //Give them a weapon and armor. These should be basic.
            Debug.Log(equippedGear.Length + " , " + (availableItems != null));
            equippedGear[0] = new ItemSave(1,"Leather Armor",availableItems[4]);
            equippedGear[1] = new ItemSave(1,"Basic Sword",availableItems[3]);
            //Do not give them a spell
        }
        //I do need to send this to state if this is the player.

    }


    public void applyUpgrade(int[] addedStats) {
        //health, attack, defense, speed, magic
        float[] newStats = stats.getStatArray();
        float prevXP = stats.experience;
        newStats[0] += addedStats[0]; // hp
        newStats[1] += addedStats[1]; // atk
        newStats[2] += addedStats[2]; // def
        newStats[3] += addedStats[3]; // spd
        newStats[4] += addedStats[4]; // mgk

        remainingHP += 10*(newStats[0] - stats.health); //Add whatever HP they obtained in the lvl up
        remainingMP += 5*(newStats[4] -stats.magic);    //Add whatever MP they obtained in the lvl up
        maximumHP = 10*newStats[0];                     //Set the new maximum HP
        maximumMP = 5*newStats[4];                      //Set the new maximum MP
        stats = new Stat(stats.level, newStats[0],newStats[1],newStats[2],newStats[3], newStats[4]);
        stats.experience = prevXP;
    }
}
