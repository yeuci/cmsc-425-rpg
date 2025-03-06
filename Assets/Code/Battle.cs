using UnityEngine;

public class Battle : MonoBehaviour
{
    public Entity playerEntity, enemyEntity;
    Stat player, enemy;
    bool playerMove;

    float playerHealth, enemyHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMove = true;

        player = playerEntity.stats;
        enemy = enemyEntity.stats;

        playerHealth = player.health;
        enemyHealth = enemy.health;

        Debug.Log("Player moves first. Select move: 1 - attack, 2 - heal, 3 - flee, 4 - item");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove) {
            Debug.Log("Player has " + playerHealth +" health remaining.");
            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                attack(playerMove, 90);
                Debug.Log("Player attacked enemy for " + player.attack + " damage!");
                Debug.Log("Enemy has " + enemyHealth +" health remaining.");
                if(enemyHealth <= 0) {
                    float enemyXP = enemyEntity.calculateXPValue();
                    Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");
                    player.experience += enemyXP;

                    playerEntity.recalculateLvl();
                    Debug.Log("Player is Lvl " + player.level + "! Progress: " + player.experience + "/"+player.expToNext);
                }
            }
        } else {
            Debug.Log("Enemy has " + enemyHealth +" health remaining.");
            if(playerHealth <= 0) {
                Debug.Log("Battle Lost");
            }
        }
    }
    
    public void attack(bool playerTurn, float dmg) {
        if (playerTurn) {
            enemyHealth -= dmg;
        } else {
            playerHealth -= dmg;
        }
    }
}
