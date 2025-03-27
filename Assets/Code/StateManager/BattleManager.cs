using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public Entity playerEntity, enemyEntity;
    Stat player, enemy;
    bool playerMove;

    float playerHealth, enemyHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerEntity = PlayerManager.player.entity();
        enemyEntity = this.AddComponent<Entity>(); // create enemy entity on battle start based on player level
        enemyEntity.scaleStats(ScalingMethod.PLAYER_LEVEL);

        player = playerEntity.stats;
        enemy = enemyEntity.stats;

        playerMove = false;
        if (player.speed >= enemy.speed) playerMove = true;

        playerHealth = playerEntity.remainingHP;
        enemyHealth = enemyEntity.remainingHP;

        Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
    }

    // Update is called once per frame
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
