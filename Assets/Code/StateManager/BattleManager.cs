using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleOption {
        ATTACK = 0,
        MAGIC = 1,
        RUN = 2,
        POTION = 3
}

public class BattleManager : MonoBehaviour
{
    public AnimationManager animationManager;
    public OpenMinigame minigame;
    public DamagePopupGenerator popupGenerator;
    Entity playerEntity, enemyEntity;
    public GameObject enemyGameObject;
    Stat player, enemy;
    Battle battle;
    bool playerMove;
    System.Func<bool> isEnemyMove;
    public Item usedItem;
    bool minigameSuccess;
    // Blocks UI when minigame is starting
    public GameObject UIBlocker;

    [HideInInspector] public PlayerManager playerManager;
    // Player and enemy health and mana bars
    public Image playerHealthBar, playerManaBar;
    public Image enemyHealthBar;
    public Canvas healthstuff;

    // Tracks escape attempts for Run option
    float escapeAttempts;
    [HideInInspector] private InventoryManager inventoryManager;

    void Awake()
    {
        if(enemyEntity != null) {
            Destroy(enemyEntity);
        }   
        
        enemyEntity = enemyGameObject.GetComponent<Entity>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        playerEntity = PlayerManager.player.entity();
        playerEntity.transform.position = new Vector3(-3.25f, 0.5f, 0);
        playerEntity.transform.right = Vector3.left;
        
        player = playerEntity.getAdjustedStats();
        enemy = enemyEntity.getAdjustedStats();
        
        battle = new Battle(playerEntity, enemyEntity, usedItem, popupGenerator);

        playerHealthBar.fillAmount = playerEntity.remainingHP / player.health;

        escapeAttempts = 0;

        Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        playerMove = player.speed >= enemy.speed;
        String msg = " has higher speed stat, and is going first";
        if(playerMove) {
            Debug.Log("PLAYER"+msg);
        } else {
            Debug.Log("ENEMY"+msg);
        }
        isEnemyMove = () => !playerMove;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        StartCoroutine(StalledUpdate());
    }

    void Update()
    {
        isEnemyMove = () => !playerMove;

        // check if player is dead
        checkEnemyDeath();

        // check if enemy is dead
        checkDeath();
    }


    IEnumerator StalledUpdate() {
        yield return new WaitUntil(isEnemyMove);
        yield return new WaitForSeconds(1);
        determineEnemyAction();
        playerMove = true;
        StartCoroutine(StalledUpdate());
    }



    public void playerAttack() {
        if(playerMove) {
            usedItem.actionType = ActionType.Attack;

            animationManager.Animate(BattleOption.ATTACK);
            battle.perform(BattleOption.ATTACK);
            AudioSource swordSwipe = GetComponent<AudioSource>();
            swordSwipe.Play();
            //battle.endTurn();

            recalculateEnemyHealthBar();
        
            checkDeath();
    
            playerMove = false;
        }
    }

    void recalculateEnemyHealthBar() {
        enemyHealthBar.fillAmount = enemyEntity.remainingHP / enemy.health;
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

    void checkEnemyDeath() {
        if(playerEntity.remainingHP <= 0) {
            Debug.Log("Player has lost the battle");

            // TODO: Add death screen NOT load screen
            SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DungeonMap")
        {
            GameObject playerTransform = GameObject.FindGameObjectWithTag("Player");
            playerTransform.transform.position = new Vector3(playerManager.enemyPositionBeforeCombat.x, playerManager.enemyPositionBeforeCombat.y, playerManager.enemyPositionBeforeCombat.z);
            
            // find and remove defeated enemy
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag("Enemy"))
                {
                    Entity entity = obj.GetComponent<Entity>();
                    if (entity != null)
                    {
                        if (entity.enemyId == playerManager.enemyBeforeCombat) {
                            obj.gameObject.SetActive(false);
                            Destroy(obj);
                        }
                    }
                }
            }

            playerManager.StartCoroutine(playerManager.DelayedDungeonRestore());

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void playerRun() {
        if(playerMove){
            animationManager.Animate(BattleOption.RUN);
            battle.perform(BattleOption.RUN);
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
        UIBlocker.SetActive(true);
        yield return minigame.StartMinigame();

        // Check the result of the minigame
        minigameSuccess = minigame.isMinigameSuccessful;

        if (minigameSuccess) {
                usedItem.actionType = ActionType.Cast;
                battle.perform(BattleOption.MAGIC);

                recalculateEnemyHealthBar();
        
                checkDeath();
        } else {
            battle.endTurn();
        }

        //battle.endTurn();

        playerMove = false;
        UIBlocker.SetActive(false);

        minigameSuccess = false;

        yield break;
    }

    public void playerPotion(){
        if(playerMove) {
            animationManager.Animate(BattleOption.POTION);
            battle.perform(BattleOption.POTION);
            //battle.endTurn();

            playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;

            playerMove = false;
        }
    }
    
    
//Enemy Actions

    public void determineEnemyAction() {
        Item bestOffense = null;
        float highestDamage = 0f;
        float bestHealing = 0f;
        bool canHeal = false;
        //Step 1: Get Best offensive action.
        foreach(Item i in enemyEntity.equippedGear) {
            if(i.actionType == ActionType.Consume) {
                canHeal = true;
                if(i.healing > bestHealing) {
                    bestHealing = i.healing;
                }
            } else {
                battle.setUsedItem(i);
                if(battle.returnDamage() > highestDamage) {
                    highestDamage = battle.returnDamage();
                    bestOffense = i;
                }
            }
            
        }
        if(playerEntity.remainingHP <= highestDamage) { //If I can kill the player this turn
            battle.setUsedItem(bestOffense);
            if(bestOffense.actionType == ActionType.Attack) {
                battle.perform(BattleOption.ATTACK);
            } else {
                battle.perform(BattleOption.MAGIC);
            }
            playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;
        } else if (enemyEntity.remainingHP/enemy.health <= 0.1f && canHeal) { //If I am below 10% health and I have a healing potion
            enemyEntity.remainingHP += Mathf.Min(enemy.health, enemyEntity.remainingHP+bestHealing);
            //Decrement the number of healing potions in my inventory.
        } else { //Make my best attack
            battle.setUsedItem(bestOffense);
            Debug.Log("ATTACKING FROM ENEMY");
            if(bestOffense.actionType == ActionType.Attack) {
                battle.perform(BattleOption.ATTACK);
            } else {
                battle.perform(BattleOption.MAGIC);
            }
            playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;
        }
    }
    public void enemyAttack() {
        usedItem.actionType = ActionType.Attack;
        Debug.Log("ATTACKING FROM ENEMY");
        battle.perform(BattleOption.ATTACK);
        //battle.endTurn();

        playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;
    }
}
