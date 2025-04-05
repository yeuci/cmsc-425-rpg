using UnityEngine;

public class EncounterResolve : MonoBehaviour
{
    public Entity attacker;
    public Entity defender;
    public Item usedItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get adjusted stats for attacker and defender for purposes of calculation
        //Attack, Defense, Health, Magic, Speed
        Stat attackerStats = attacker.getAdjustedStats();
        Stat defenderStats = defender.getAdjustedStats();

        //User heals HP set by the item
        //We need to separate current and maximum health for me to bind this.
        attacker.stats.health += usedItem.health;

        //User deals damage equal to weapon power * player attack / enemy defense
        if(usedItem.actionType == ActionType.Attack){
            defender.remainingHP -= attackerStats.attack*usedItem.attackPower/defenderStats.defense;
        } else if (usedItem.actionType == ActionType.Cast) {
            defender.remainingHP -= attackerStats.magic*usedItem.magicPower;
        }
    }
}
