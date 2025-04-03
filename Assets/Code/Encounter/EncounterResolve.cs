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
        float [] attackerStats = attacker.getAdjustedStats();
        float [] defenderStats = defender.getAdjustedStats();

        //User heals HP set by the item
        //We need to separate current and maximum health for me to bind this.
        attacker.stats.health += usedItem.health;

        //User deals damage equal to weapon power * player attack / enemy defense
        if(usedItem.actionType == ActionType.Attack){
            defender.remainingHP -= attackerStats[0]*usedItem.attackPower/defenderStats[1];
        } else if (usedItem.actionType == ActionType.Cast) {
            defender.remainingHP -= attackerStats[3]*usedItem.magicPower;
        }
    }
}
