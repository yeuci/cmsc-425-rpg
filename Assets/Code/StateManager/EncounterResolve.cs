using UnityEngine;

public class EncounterResolve
{
    Entity attacker;
    Entity defender;
    public Item usedItem;

    Stat attackerStats, defenderStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public EncounterResolve(Entity a, Entity d, Item uI) {
        attacker = a;
        defender = d;
        usedItem = uI;

        attackerStats = attacker.getAdjustedStats();
        defenderStats = defender.getAdjustedStats();
    }

    public void setAttacker(Entity a) {
        attacker = a;
        attackerStats = attacker.getAdjustedStats();
    }

    public void setDefender(Entity d) {
        defender = d;
        defenderStats = defender.getAdjustedStats();
    }
    
    public float returnDamage() {
        float damage = 0f;
        if(usedItem.actionType == ActionType.Attack){
            damage = attackerStats.attack*usedItem.attackPower/defenderStats.defense;
        } else if (usedItem.actionType == ActionType.Cast) {
            damage = attackerStats.magic*usedItem.magicPower;
        }

        return damage;
    }
    


}
