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
        player = playerEntity.stats;
        enemy = enemyEntity.stats;

        playerMove = false;
        if (player.speed >= enemy.speed) playerMove = true;

        playerHealth = playerEntity.remainingHP;
        enemyHealth = enemyEntity.remainingHP;

        Debug.Log("Player moves first. Select move: 1 - attack, 2 - heal, 3 - flee, 4 - item");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove) {
            Debug.Log("Player has " + playerHealth +" health remaining.");
            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                attack(playerEntity, enemyEntity, 9);
                Debug.Log("Player attacked enemy for " + player.attack + " damage!");
                Debug.Log("Enemy has " + enemyHealth +" health remaining.");
                if(enemyHealth <= 0) {
                    float enemyXP = enemyEntity.calculateXPValue();
                    Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");
                    player.experience += enemyXP;

                    playerEntity.recalculateLvl();
                    Debug.Log("Player is Lvl " + player.level + "! Progress: " + player.experience + "/"+player.expToNext);
                }
                playerMove = false;
            }
        } else {
            Debug.Log("Enemy has " + enemyHealth +" health remaining.");
            attack(playerEntity, enemyEntity, 9);
            Debug.Log("Enemy attacked player for " + enemy.attack * 9 + " damage!");
            Debug.Log("Player has " + playerHealth +" health remaining.");
            if(playerHealth <= 0) {
                Debug.Log("Battle Lost");
                
            }
        }
    }
    
    public void attack(Entity attacker, Entity defender, float dmg) {
        defender.remainingHP -= dmg * attacker.stats.attack;
    }

    public void playerAttack() {
        enemyEntity.remainingHP -= player.attack * 5;
        Debug.Log("asldkjalskd");
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
}
