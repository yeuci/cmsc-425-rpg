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

    public Color DMGCOLOR = Color.red;
    public Color HEALCOLOR = Color.green;
    public Color MANACOLOR = Color.blue;

    
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

    public void setUsedItem(Item uI) {
        usedItem = uI;
        attackerStats = attacker.getAdjustedStats();
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
            case BattleOption.USE_ITEM:
                Debug.Log(usedItem);
                if(usedItem.actionType == ActionType.Attack){
                    float damage =  attackerStats.attack*usedItem.attackPower/defenderStats.defense;
                    defender.remainingHP -= damage;

                    popupGenerator.CreatePopUp(defender.transform.position, damage.ToString(), defender.transform.right, DMGCOLOR);
                } else if (usedItem.actionType == ActionType.Cast) {
                    float damage = attackerStats.magic*usedItem.magicPower;
                    defender.remainingHP -= damage;

                    popupGenerator.CreatePopUp(defender.transform.position, damage.ToString(),defender.transform.right, DMGCOLOR);
                } else if (usedItem.actionType == ActionType.Consume) {
                    float healing = 0;
                    float manaRestore = 0;
                    if (usedItem.healing > 0) {
                        healing = usedItem.healing;
                    }

                    if (usedItem.manaRestore > 0) {
                        manaRestore = usedItem.manaRestore;
                    }

                    attacker.remainingHP = Mathf.Min(attackerStats.health,attacker.remainingHP+usedItem.healing);
                    attacker.remainingMP = Mathf.Min(attackerStats.mana,attacker.remainingMP+usedItem.manaRestore);

                    if (healing > 0) {
                        popupGenerator.CreatePopUp(attacker.transform.position, healing.ToString(),attacker.transform.right, HEALCOLOR);
                    }
                    if (manaRestore > 0) {
                        popupGenerator.CreatePopUp(attacker.transform.position,manaRestore.ToString(),attacker.transform.right, MANACOLOR);
                    }
                }

                Debug.Log("Ending turn. Old Attacker: "+attacker.name);
                endTurn();
                Debug.Log("Attacker: "+attacker.name);
                break;
            
            case BattleOption.RUN:
                endTurn();
                break;
            case BattleOption.POTION:
                float healAmount = 10f;
                attacker.remainingHP += healAmount;
                if (attacker.remainingHP > attackerStats.health) {
                    attacker.remainingHP = attackerStats.health;
                }
                popupGenerator.CreatePopUp(attacker.transform.position, healAmount.ToString(),attacker.transform.right, HEALCOLOR);
                endTurn();
                break;
        }
    }

    public void endTurn() {
        (attacker, defender) = (defender, attacker);
    }
}
