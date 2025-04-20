using Unity.VisualScripting;
using UnityEngine;

public class Battle
{
    bool resolved;
    Entity attacker;
    Entity defender;
    public Item usedItem;
    DamagePopupGenerator popupGenerator;

    Stat attackerStats, defenderStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Battle(Entity a, Entity d, Item uI, DamagePopupGenerator popupGen) {
        attacker = a;
        defender = d;
        usedItem = uI;
        popupGenerator = popupGen;

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

    public void perform(BattleOption battleOption) {
        switch (battleOption) {
            case BattleOption.ATTACK:
                if(usedItem.actionType == ActionType.Attack) {
                    float damage =  attackerStats.attack + attackerStats.attack*usedItem.attackPower/defenderStats.defense;
                    defender.remainingHP -= damage;

                    popupGenerator.CreatePopUp(defender.transform.position, damage.ToString(), defender.transform.right);
                }
                break;        
            case BattleOption.MAGIC:
                if (usedItem.actionType == ActionType.Cast) {
                    float damage = attackerStats.magic + attackerStats.magic*usedItem.magicPower;
                    defender.remainingHP -= damage;

                    popupGenerator.CreatePopUp(defender.transform.position, damage.ToString(),defender.transform.right);
                }
                break;
            case BattleOption.RUN:
                break;
            case BattleOption.POTION:
                attacker.remainingHP += 10;
                if (attacker.remainingHP > attackerStats.health) {
                    attacker.remainingHP = attackerStats.health;
                }
                break;
        }
    }

    public void endTurn() {
        (attacker, defender) = (defender, attacker);
    }
}
