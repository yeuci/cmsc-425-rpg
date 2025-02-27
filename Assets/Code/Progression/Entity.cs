using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    [SerializeField] public Stat stats;
    [SerializeField] public Class eClass;

    bool isAlive = true;

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
    public void attackEntity(Entity entity, float dmg) {
        entity.stats.health -= dmg;
        if (entity.stats.health <= 0) {
            kill(entity);
        }
    }

    public void kill(Entity entity) {
        stats.experience += entity.calculateXPDropped();
        recalculateLvl();
    }
    public float calculateXPDropped() {
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
        if (stats.health <= 0) {
            isAlive = false;
            return false;
        }
        return true;
    }
}
