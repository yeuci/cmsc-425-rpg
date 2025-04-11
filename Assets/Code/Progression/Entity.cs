using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public enum ScalingMethod {
        PLAYER_LEVEL,
        CLASS,
        CUSTOM
    }

[System.Serializable]
public class Entity : MonoBehaviour
{
    [SerializeField] public Stat stats;
    [SerializeField] public Item[] equippedGear;
    [SerializeField] public ArrayList inventory;
    [SerializeField] public Class eClass;

    // Remaining HP of entity
    public float remainingHP;

    // Alive check (Should destroy Entity gameobject if false)
    bool isAlive = true;

    void Start() {
        remainingHP = stats.health;
    }

    // Basic Entity
    public Entity() {
        stats = new Stat();
    }

    public float calculateXPValue() {
        return stats.getStatTotal();
    }

    public void recalculateLvl() {
        if (stats.experience > stats.expToNext) {
            stats.level += 1;
            stats.experience -= stats.expToNext;
            stats.expToNext *= 2;
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
        //Attack, Defense, Health, Magic Speed
        float[] adjStats = this.stats.getStatArray();
        foreach (Item i in equippedGear){
            adjStats[0] += i.attack;
            adjStats[1] += i.defense;
            adjStats[2] += i.health;
            adjStats[3] += i.mana;
            adjStats[4] += i.speed;
        }
        Stat stats = new Stat(this.stats.level, adjStats[2],adjStats[0],adjStats[1],adjStats[4],adjStats[3]);
        return stats;
    }

    public void scaleStats(ScalingMethod scaleMethod, float[] scalings = default) {
        float constantScale = 1.0f;
        if (scalings == default) constantScale = Random.Range(0.8f, 1.2f);

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
    }
}
