using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    [SerializeField] public Stat stats;
    [SerializeField] public Item[] equippedGear;
    [SerializeField] public Class eClass;

    public float remainingHP;
    bool isAlive = true;

    void Start() {
        remainingHP = stats.health;
    }

    // Basic Entity
    public Entity() {
        stats = new Stat();
    }

    // Entity with custom stats
    public Entity(Stat customStats) {
        stats = customStats;
    }    

    public Entity(Class entityClass) {
        // Add check for class and stats based on class
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
}
