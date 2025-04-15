using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public GameObject fireball; //Remove after. This is just to test
    public OpenMinigame minigame;
    Entity playerEntity, enemyEntity;
    GameObject playerObject, enemyObject;
    Stat player, enemy;
    EncounterResolve manager;
    bool playerMove;
    System.Func<bool> isEnemyMove;
    public Item usedItem;
    float leftBound;
    float originalSize, enemyOriginalSize;
    Vector3 playerHealthBarLoc, enemyHealthBarLoc;
    bool minigameSuccess;

    public SpriteRenderer healthBar, enemyHealthBar;

    float playerHealth, enemyHealth;

    // Tracks escape attempts for Run option
    float escapeAttempts;

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

        leftBound = healthBar.transform.position.x - healthBar.size.x/2;
        originalSize = healthBar.size.x;
        enemyOriginalSize = enemyHealthBar.size.x;
        
        playerHealthBarLoc = healthBar.transform.position;
        enemyHealthBarLoc = enemyHealthBar.transform.position;

        escapeAttempts = 0;

        manager = new EncounterResolve(playerEntity, enemyEntity, usedItem );

        Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        playerMove =  player.speed >= enemy.speed;
        String msg = " has higher speed stat, and is going first";
        if(playerMove) {
            Debug.Log("PLAYER"+msg);
        } else {
            Debug.Log("ENEMY"+msg);
        }
        isEnemyMove = () => !playerMove;
        StartCoroutine(StalledUpdate());
    }

    void Update()
    {
        Debug.Log(playerMove);
        isEnemyMove = () => !playerMove;
    }


    IEnumerator StalledUpdate() {
        yield return new WaitUntil(isEnemyMove);
        yield return new WaitForSeconds(1);
        enemyAttack();
        playerMove = true;
        StartCoroutine(StalledUpdate());
    }



    public void playerAttack() {
        if(playerMove) {
            usedItem.actionType = ActionType.Attack;
            GameObject instance = Instantiate(fireball, playerEntity.transform.position,Quaternion.identity);
            manager.setAttacker(playerEntity);
            manager.setDefender(enemyEntity);

            enemyEntity.remainingHP -= manager.returnDamage();

            recalculateEnemyHealthBar();
        
            checkDeath();
    
            playerMove = false;
        }
    }

    void recalculateEnemyHealthBar() {
        enemyHealthBar.size = new Vector2(enemyOriginalSize*enemyEntity.remainingHP/enemy.health, 0.64f);
        float leftShift = (enemyOriginalSize-enemyHealthBar.size.x)*enemyOriginalSize/40;
        enemyHealthBar.transform.position = new Vector3(enemyHealthBarLoc.x-leftShift,enemyHealthBarLoc.y,enemyHealthBarLoc.z);
    }

    void checkDeath() {
        if(enemyEntity.remainingHP <= 0) {
            float enemyXP = enemyEntity.calculateXPValue();
            Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");
            player.experience += enemyXP;

            playerEntity.recalculateLvl();
            Debug.Log("Player is Lvl " + player.level + "! Progress: " + player.experience + "/"+player.expToNext);

            SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }



    public void enemyAttack() {
        Debug.Log("ATTACKING FROM ENEMY");
        manager.setAttacker(enemyEntity);
        manager.setDefender(playerEntity);

        playerEntity.remainingHP -= manager.returnDamage();
        healthBar.size = new Vector2(originalSize*playerEntity.remainingHP/player.health, 0.64f);
        float leftShift = (originalSize-healthBar.size.x)*originalSize/40;
        healthBar.transform.position = new Vector3(playerHealthBarLoc.x-leftShift,playerHealthBarLoc.y,playerHealthBarLoc.z);
        
        if(playerEntity.remainingHP <= 0) {
            Debug.Log("Player has lost the battle");
            SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    public void playerRun() {
        if(playerMove){
            if(player.speed > enemy.speed) {
                Debug.Log("Player has fled the encounter");
                SceneManager.LoadScene("Scenes/DungeonMap");
            }
            else {
                escapeAttempts += 1;
                float escapeChance = Mathf.Floor((player.speed * 32) / (enemy.speed / 4)) + 30.0f * escapeAttempts;
                float rand = UnityEngine.Random.Range(0.0f, 255.0f);
                if (rand < escapeChance) 
                {
                    Debug.Log("Player has fled the encounter");
                    SceneManager.LoadScene("Scenes/DungeonMap");
                }
                else 
                {
                    Debug.Log("Enemy was too fast, player failed to flee the encounter");
                    playerMove = false;
                }
            }
        }
    }

    public void playerCast() {
        if (playerMove) {
            StartCoroutine(HandlePlayerCast());
        }
    }

    // Coroutine so it waits for minigame to finish before moving on
    private IEnumerator HandlePlayerCast() {
        // Wait for the minigame to finish
        yield return minigame.StartMinigame();

        // Check the result of the minigame
        minigameSuccess = minigame.isMinigameSuccessful;

        if (minigameSuccess) {
                usedItem.actionType = ActionType.Attack;
                enemyEntity.remainingHP -= manager.returnDamage();
                
                recalculateEnemyHealthBar();
        
                checkDeath();
        }

        playerMove = false;

        yield break;
    }

    public void playerPotion(){
        if(playerMove) {
            playerEntity.remainingHP += 10;
            if (playerEntity.remainingHP > player.health) {
                playerEntity.remainingHP = player.health;
            }
            healthBar.size = new Vector2(originalSize*playerEntity.remainingHP/player.health, 0.64f);
            float leftShift = (originalSize-healthBar.size.x)*originalSize/40;
            healthBar.transform.position = new Vector3(playerHealthBarLoc.x-leftShift,playerHealthBarLoc.y,playerHealthBarLoc.z);
            playerMove = false;
        }
    }
    
    
}
