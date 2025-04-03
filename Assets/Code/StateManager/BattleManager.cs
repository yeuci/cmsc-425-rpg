using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    Entity playerEntity, enemyEntity;
    GameObject playerObject, enemyObject;
    Stat player, enemy;
    bool playerMove;

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

        player = playerEntity.stats;
        enemy = enemyEntity.stats;

        Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
    }

    void Update()
    {
        if(!playerMove) {
            enemyAttack();
            playerMove = true;
        }
    }

    public void playerAttack() {
        enemyEntity.remainingHP -= player.attack * 5;
        Debug.Log("Player attacked enemy for " + player.attack * 5 + " damage!");     
        Debug.Log("PLAYER ATTACK!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
    
        if(enemyEntity.remainingHP <= 0) {
            float enemyXP = enemyEntity.calculateXPValue();
            Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");
            player.experience += enemyXP;

            playerEntity.recalculateLvl();
            Debug.Log("Player is Lvl " + player.level + "! Progress: " + player.experience + "/"+player.expToNext);

            SceneManager.LoadScene("Scenes/EncounterScene");
        }
   
        playerMove = false;
    }

    public void enemyAttack() {
        playerEntity.remainingHP -= enemy.attack;
        Debug.Log("Enemy attacked player for " + enemy.attack + " damage!");
        Debug.Log("ENEMY ATTACK!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        if(playerEntity.remainingHP <= 0) {
            Debug.Log("Player has lost the battle");
            SceneManager.LoadScene("Scenes/EncounterScene");
        }
    }
    
}
