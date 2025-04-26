using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleOption {
        ATTACK = 0,
        MAGIC = 1,
        RUN = 2,
        POTION = 3,
        USE_ITEM = 4
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    
    Entity playerEntity, enemyEntity;       // Player and enemy entity
    Stat player, enemy;                     // Player and enemy stats
    Battle battle;                          // Manages battle actions
    bool playerMove;                        // Track if player can move
    System.Func<bool> isEnemyMove;          // Track if enemy can move
    bool minigameSuccess;                   // Tracks if minigame is successfull
    float escapeAttempts;                   // Tracks escape attempts for Run option
    List<ItemSave> spells = new List<ItemSave>();   // Tracks which spells the player has access to
    

    // Managers
    public AnimationManager animationManager;
    public OpenMinigame minigame;
    public DamagePopupGenerator popupGenerator;

    public GameObject enemyGameObject;
    public Item usedItem;

    public Canvas battleCanvas;
    public GameObject UIBlocker;              // Blocks UI when minigame is starting
    public GameObject inventoryPanel;
    

    [HideInInspector] public PlayerManager playerManager;

    // Player and enemy health and mana bars
    public Image playerHealthBar, playerManaBar;
    public Image enemyHealthBar;
    public Canvas healthstuff;

    [HideInInspector] private InventoryManager inventoryManager;
    public GameObject spellButtonPrefab; // Drag the prefab here in the Inspector
    public Transform spellListContainer;
    public GameObject spellInfoPrefab;
    private GameObject currentSpellInfo = null;

    void getPlayerSpells() {
        ItemSave[] playerInventory = playerEntity.inventory;

        for (int i =0; i < playerInventory.Length; ++i) {
            if (playerInventory[i] != null && playerInventory[i].itemData != null) {
                ItemSave item = playerInventory[i];

                if (item.itemData.type == ItemType.Spell) {
                    spells.Add(item);
                }
            }
        }        
    }

    void Awake()
    {
        if(enemyEntity != null) {
            Destroy(enemyEntity);
        }   
        
        instance = this;
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

        getPlayerSpells();
        
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

        // GetSpells();

        // Debug.Log(spells.Count);
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
        enemyArtificialIntelligence();
        playerMove = true;
        StartCoroutine(StalledUpdate());
    }



    public void playerAttack() {
        if(playerMove) {
            usedItem.actionType = ActionType.Attack;

            animationManager.Animate(BattleOption.ATTACK);
            battle.perform(BattleOption.USE_ITEM);
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
            playerManager.playerCanCollide = false;

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

            playerManager.playerCanCollide = true;

            playerManager.defeatedEnemies.Add(playerManager.enemyBeforeCombat);
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
        // if (playerMove) {
        //     StartCoroutine(HandlePlayerCast());
        // }

        if (playerMove) {
            displaySpellButtons();
        }
    }

    // Coroutine so it waits for minigame to finish before moving on
    private IEnumerator HandlePlayerCast() {
        // Wait for the minigame to finish
        UIBlocker.SetActive(true);
        yield return StartCoroutine(minigame.StartMinigame());

        // Check the result of the minigame
        minigameSuccess = minigame.isMinigameSuccessful;

        if (minigameSuccess) {
                usedItem.actionType = ActionType.Cast;
                battle.perform(BattleOption.USE_ITEM);

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
            battle.perform(BattleOption.USE_ITEM);
            //battle.endTurn();
            playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;
            playerMove = false;
        }
    }
    
    
//Enemy Action
    //The goal is to have the AI make less mistakes as the player becomes more powerful.
    public void enemyArtificialIntelligence() {
        System.Random rd = new System.Random();
        int rand_num = rd.Next(1,10);
        if(rand_num <= player.level) {
            //Take a random action. This works if the player can hit level 10.
            Item itemPick = enemyEntity.inventory[rd.Next(0,enemyEntity.inventoryCount)].itemData;
            battle.setUsedItem(itemPick);
        } else {
            Item bestDamage = null;
            Item bestHealing = null;
            float maxDamage = 0f;
            float maxHealing = 0f;
            //Use equipped gear to determine best healing and best damage
            if(enemyEntity.equippedGearCount == 0) {
                Debug.Log("No equipped items found. Running");
                //battle.perform(BattleOption.RUN);
            }
            foreach (ItemSave iS in enemyEntity.equippedGear){
                if (iS != null && iS.itemData != null) {
                    Item i = iS.itemData;
                    if(i != null) {
                        if(i.healing > maxHealing) {
                            bestHealing = i;
                            maxHealing = i.healing;
                        }
                        battle.setUsedItem(i);
                        if(battle.returnDamage() > maxDamage) {
                            maxDamage = battle.returnDamage();
                            bestDamage = i;
                        }
                    }
                }
            }
            //Determine the course of action
            if(playerEntity.remainingHP <= maxDamage) { //If I can kill the player this turn, do it
                Debug.Log("Can Kill player. Max damage is "+maxDamage );
                battle.setUsedItem(bestDamage);
            } else if (enemyEntity.remainingHP/enemy.health <= 0.1f && maxHealing > 0f) { //I am at at < 10% HP and can heal
                Debug.Log("At risk of death. Healing now");
                battle.setUsedItem(bestHealing);
            } else {
                Debug.Log("Not at risk of death. Attacking");
                battle.setUsedItem(bestDamage);
            }
        }

        //Temporary fix to prevent game from breaking
        usedItem.actionType = ActionType.Attack;
        battle.setUsedItem(usedItem);
        //End of temporary fix.
        battle.perform(BattleOption.USE_ITEM);
        playerHealthBar.fillAmount  = playerEntity.remainingHP / player.health;
        recalculateEnemyHealthBar(); 
    }

    void displaySpellButtons()
    {
        foreach (Transform child in spellListContainer)
        {
            Destroy(child.gameObject); // Clear existing buttons
        }

        foreach (ItemSave spell in spells)
        {
            Item item = spell.itemData;

            GameObject buttonObj = Instantiate(spellButtonPrefab, spellListContainer);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = item.name;

            HoverSpellButton hoverSpellButton = buttonObj.GetComponent<HoverSpellButton>();
            hoverSpellButton.item = item;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => {
                usedItem = item;
                battle.setUsedItem(usedItem);

                OpenMinigame minigameOpenerInstance = Instantiate(item.minigameOpener);
                
                if (minigame != null) {
                    Destroy(minigame);
                }
                minigame = minigameOpenerInstance;
                minigame.minigamePrefab = item.minigame;
                minigame.canvas = battleCanvas;

                minigame.gameObject.SetActive(true);

                StartCoroutine(HandlePlayerCast());
            });
        }

    }

    public void displaySpellInformation(string itemName, string itemDescription, Vector2 buttonPos) {
        if (currentSpellInfo != null) {
            Destroy(currentSpellInfo.gameObject);
        }

        buttonPos.x += 100;
        buttonPos.y += 100;

        currentSpellInfo = Instantiate(spellInfoPrefab, buttonPos, Quaternion.identity, battleCanvas.transform);
        currentSpellInfo.GetComponent<SpellPopupInfo>().Setup(itemName, itemDescription);
    }

    public void DestroyItemInfo()
    {
        if(currentSpellInfo != null)
        {
            Destroy(currentSpellInfo.gameObject);
        }
    }

}
