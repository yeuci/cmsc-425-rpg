using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject fireball; //Remove after. This is just to test
    Entity playerEntity, enemyEntity;
    GameObject playerObject, enemyObject;
    Stat player, enemy;
    EncounterResolve manager;
    bool playerMove;
    public Item usedItem;

    float playerHealth, enemyHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyObject = GameObject.FindGameObjectWithTag("Enemy");

        playerEntity = playerObject.GetComponent<Entity>();
        enemyEntity = enemyObject.GetComponent<Entity>();
        //So far, I gain access to the GameObjects and their Entity components. I now need to work with them.

        //MISSING: ADJUST scaleStats TO SET UP THE STATS CORRECTLY

        player = playerEntity.getAdjustedStats();
        enemy = enemyEntity.getAdjustedStats();
        

        manager = new EncounterResolve(playerEntity, enemyEntity, usedItem );

        Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        playerMove =  player.speed >= enemy.speed;
        String msg = " has higher speed stat, and is going first";
        if(playerMove) {
            Debug.Log("PLAYER"+msg);
        } else {
            Debug.Log("ENEMY"+msg);
        }
    }

    void Update()
    {
        if(!playerMove) {
            enemyAttack();
            playerMove = true;
        }
    }

    public void playerAttack() {
        GameObject instance = Instantiate(fireball, playerEntity.transform.position,Quaternion.identity);
        manager.setAttacker(playerEntity);
        manager.setDefender(enemyEntity);

        enemyEntity.remainingHP -= manager.returnDamage();
        Debug.Log("Player attacked enemy for " + manager.returnDamage() + " damage!");     
        Debug.Log("PLAYER ATTACK!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
    
        if(enemyEntity.remainingHP <= 0) {
            float enemyXP = enemyEntity.calculateXPValue();
            Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");
            player.experience += enemyXP;

            playerEntity.recalculateLvl();
            Debug.Log("Player is Lvl " + player.level + "! Progress: " + player.experience + "/"+player.expToNext);

            SceneManager.LoadScene("Scenes/DungeonMap");
        }
   
        playerMove = false;
    }

    public void enemyAttack() {
        manager.setAttacker(enemyEntity);
        manager.setDefender(playerEntity);

        playerEntity.remainingHP -= manager.returnDamage();
        Debug.Log("Enemy attacked player for " + manager.returnDamage() + " damage!");
        Debug.Log("ENEMY ATTACK!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        if(playerEntity.remainingHP <= 0) {
            Debug.Log("Player has lost the battle");
            SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    public void playerRun() {
        playerMove = false;
        if(player.speed > enemy.speed) {
            Debug.Log("Player has fled the encounter");
            SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    public void playerCast() {
        Debug.Log("Magic Clicked");
        playerMove = false;
    }

    public void playerPotion(){
        Debug.Log("Player drank a potion");
        playerMove = false;
    }
    
    
}
