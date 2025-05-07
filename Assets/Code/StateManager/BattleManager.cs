using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
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
    
    public Entity playerEntity, enemyEntity;                   // Player and enemy entity
    public Stat player, enemy;                                 // Player and enemy stats
    public Battle battle;                                      // Manages battle actions
    public bool playerMove;                                    // Track if player can move
    System.Func<bool> isEnemyMove;                      // Track if enemy can move
    bool minigameSuccess;                               // Tracks if minigame is successfull
    float escapeAttempts;                               // Tracks escape attempts for Run option
    List<ItemSave> spells = new List<ItemSave>();       // Tracks which spells the player has access to
    List<ItemSave> consumables = new List<ItemSave>();  // Tracks potions player has

    // Managers
    public AnimationManager animationManager;
    public DamagePopupGenerator popupGenerator;         // Creates damage popups
    public AudioManager musicManager;

    public GameObject enemyGameObject;
    public Item usedItem;

    public Canvas battleCanvas;
    public GameObject UIBlocker;                        // Blocks UI when minigame is starting

    public Transform battleTextContainer;
    public GameObject battleTextPanel;
    

    [HideInInspector] public PlayerManager playerManager;

    // Player and enemy health and mana bars
    public Image playerHealthBar, playerManaBar;
    public Image enemyHealthBar;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerManaText;


    // Invetory and Spells    
    public GameObject inventoryButtonPrefab; // Drag the prefab here in the Inspector
    public Transform spellListContainer;
    public GameObject inventoryInfoPrefab;
    private GameObject currentItemInfo = null;
    public Transform inventoryPopupContainer;

    // Minigames
    public OpenMinigame minigame;
    PlaySpellAnimation spellAnimationPlayer;

    // Game over screen
    GameObject levelChanger;
    public DeathMenuManager gameOverScreen;
    bool isGameOver;

    // Results
    public ResultsWindow results;
    public bool isEnemyReady = false;
    public bool alreadyCalled = false;

    void getPlayerInventory() {
        ItemSave[] playerInventory = playerEntity.inventory;

        for (int i =0; i < playerInventory.Length; ++i) {
            if (playerInventory[i] != null && playerInventory[i].itemData != null) {
                ItemSave item = playerInventory[i];

                if (item.itemData.type == ItemType.Spell) {
                    spells.Add(item);
                } else if (item.itemData.type == ItemType.Consumable) {
                    consumables.Add(item); // Add the reference to the consumables list
                }

                
            }
        }        
    }

    void attachEquipment() {
        GameObject torso = GameObject.FindGameObjectWithTag("Torso");
        GameObject rightHand = GameObject.FindGameObjectWithTag("MainHand");
        GameObject leftHand = GameObject.FindGameObjectWithTag("OffHand");

        Debug.Log(leftHand);
        

        if (playerEntity.equippedGear[0]?.itemData?.material != null) {
            GetPlayerItems.setArmorMaterial(torso, playerEntity.equippedGear[0].itemData.material);
        }
        if (playerEntity.equippedGear[1]?.itemData?.itemPrefab != null) {
            GetPlayerItems.attachSword(rightHand, playerEntity.equippedGear[1].itemData.itemPrefab);
        }
        if (playerEntity.equippedGear[2]?.itemData?.itemPrefab != null) {
            GetPlayerItems.attachShield(leftHand, playerEntity.equippedGear[2].itemData.itemPrefab);
        }
    }

    void Awake()
    {
        Debug.Log("AN INSTANCE OF BATTLE MANAGER IS CREATED");
        if(enemyEntity != null) {
            Destroy(enemyEntity);
        }   

        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // enemyEntity = enemyGameObject.GetComponent<Entity>();
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        playerEntity = PlayerManager.player.entity();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        musicManager = GameObject.FindGameObjectWithTag("MusicManager")?.GetComponent<AudioManager>();
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger");
        battleTextPanel.SetActive(false);
        
        playerEntity.transform.position = new Vector3(-3.25f, 0.5f, 0);
        playerEntity.transform.right = Vector3.left;

        attachEquipment();
        getPlayerInventory();
        
        player = playerEntity.getAdjustedStats();
        // enemy = enemyEntity.getAdjustedStats();
        
        // battle = new Battle(playerEntity, enemyEntity, usedItem, popupGenerator);

        updatePlayerHealthAndManaBar();

        escapeAttempts = 0;

        playerHealthText.text = $"Health: {playerEntity.remainingHP} / {playerEntity.maximumHP}";
        playerManaText.text = $"Mana: {playerEntity.remainingMP} / {playerEntity.maximumMP}";

        // Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + enemyEntity.remainingHP + "/" + enemy.health+" - Player HP: "+playerEntity.remainingHP+"/"+player.health);
        // playerMove = player.speed >= enemy.speed;
        String msg = " has higher speed stat, and is going first";
        if(playerMove) {
            Debug.Log("PLAYER"+msg);
        } else {
            Debug.Log("ENEMY"+msg);
        }
        isEnemyMove = () => !playerMove;

        // GetSpells();

        // Debug.Log(spells.Count);
        // if (isEnemyReady) {
        //     StartCoroutine(StalledUpdate());
        // }

        playerManager.getEnemyEntity();

        Debug.Log(enemyEntity.remainingHP);
    }

    void Update()
    {
        // Debug.Log(enemyEntity.remainingHP);
        if (!isEnemyReady) {
            return;
        } else if (isEnemyReady && !alreadyCalled) {
            Debug.Log(enemyEntity.remainingHP);
            StartCoroutine(StalledUpdate());
            alreadyCalled = true;
            // Debug.Log(enemyEntity.remainingHP);
        }

        isEnemyMove = () => !playerMove;

        // check if player is dead
        checkDeath();
    }

    IEnumerator StalledUpdate() {
        while (enemyEntity.remainingHP > 0) {
            yield return new WaitUntil(isEnemyMove);
            UIBlocker.SetActive(true);

            battleTextPanel.SetActive(false);

            if (enemyEntity.remainingHP > 0) {
                yield return new WaitForSeconds(1);
                if (currentItemInfo != null) {
                    Destroy(currentItemInfo.gameObject);
                }

                battleTextPanel.SetActive(true);

                improvedEnemyAI();
                yield return new WaitForSeconds(1f);
                battleTextPanel.SetActive(false);

                playerMove = true;
                UIBlocker.SetActive(false);
            }
            
        }
    }

    public void playerAttack() {
        if(playerMove) {
            musicManager.PlayConfirmed();
            usedItem = playerEntity.equippedGear[1].itemData;
            //Set attack to unarmed strike if they don't have a weapon equipped.
            if(usedItem == null) {
                usedItem = GetComponentInParent<AvailableItemsAccess>().availableItems[9];
            }

            battle.setUsedItem(usedItem); //Set the used item to the weapon that the player currently has equipped
            battle.perform(BattleOption.USE_ITEM);
            AudioSource swordSwipe = GetComponent<AudioSource>();
            swordSwipe.Play();

            recalculateEnemyHealthBar();
        
            checkEnemyDeath();
            
            updatePlayerHealthAndManaText();
    
            playerMove = false;
        } else {
            musicManager.PlayDenied();
        }
    }

    public void recalculateEnemyHealthBar() {
        enemyHealthBar.fillAmount = enemyEntity.remainingHP / enemyEntity.maximumHP;
    }

    public void updatePlayerHealthAndManaBar() {
        playerHealthBar.fillAmount = playerEntity.remainingHP / playerEntity.maximumHP;
        playerManaBar.fillAmount = playerEntity.remainingMP / playerEntity.maximumMP;
    }

    public void updatePlayerHealthAndManaText() {
        playerHealthText.text = $"Health: {playerEntity.remainingHP} / {playerEntity.maximumHP}";
        playerManaText.text = $"Mana: {playerEntity.remainingMP} / {playerEntity.maximumMP}";
    }

    void checkEnemyDeath() {
        if(enemyEntity.remainingHP <= 0) {
            float enemyXP = enemyEntity.calculateXPValue();
            Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");

            float prevXP = playerEntity.stats.experience;
            float prevCap = playerEntity.stats.expToNext;
            int prevLvl = playerEntity.stats.level;
            int prevSP = playerEntity.skillPoints;
            
            playerEntity.stats.experience += enemyXP;
            playerEntity.recalculateLvl();
            
            Debug.Log("Player is Lvl " + playerEntity.stats.level + "! Progress: " + playerEntity.stats.experience + "/"+playerEntity.stats.expToNext);

            musicManager.sceneMusic.Stop();
            musicManager.playVictory();
            UIBlocker.SetActive(true);

            StartCoroutine(results.showVictory(prevSP, prevLvl, (int)prevXP, (int)prevCap, enemyXP));
            playerManager.inCombat = false;

            // SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    IEnumerator GameOver() {
        yield return new WaitUntil(() => playerMove);
        StartCoroutine(gameOverScreen.Setup());
    }

    void checkDeath() {
        if(playerEntity.remainingHP <= 0 && !isGameOver) {
            isGameOver = true;
            Debug.Log("Player has lost the battle");

            // TODO: Add death screen NOT load screen
            StartCoroutine(GameOver());
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
            
            playerManager.defeatedEnemies.Add(playerManager.enemyBeforeCombat);
            playerManager.playerCanCollide = true;
            playerManager.StartCoroutine(playerManager.DelayedDungeonRestore());

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void playerRun() {
        StartCoroutine(handlePlayerRun());
    }

    private IEnumerator handlePlayerRun() {
        if(playerMove){
            animationManager.Animate(BattleOption.RUN);
            battle.perform(BattleOption.RUN);
            if(player.speed > enemy.speed) {
                musicManager.PlayConfirmed();
                Debug.Log("Player has fled the encounter");
                yield return StartCoroutine(displayAction("Player successfully fled!"));

                FadeTransition transition = levelChanger.GetComponent<FadeTransition>();
                transition.animator = levelChanger.GetComponent<Animator>();
                yield return StartCoroutine(transition.PlayFadeOutFast());
                playerManager.inCombat = false;
                SceneManager.LoadScene("Scenes/DungeonMap");
            }
            else {
                escapeAttempts += 1;
                float escapeChance = Mathf.Floor((player.speed * 32) / (enemy.speed / 4)) + 30.0f * escapeAttempts;
                float rand = UnityEngine.Random.Range(0.0f, 255.0f);
                if (rand < escapeChance) 
                {
                    UIBlocker.SetActive(true);
                    musicManager.PlayConfirmed();
                    Debug.Log("Player has fled the encounter with escape attempts");
                    yield return StartCoroutine(displayAction("Player successfully fled!"));

                    FadeTransition transition = levelChanger.GetComponent<FadeTransition>();
                    transition.animator = levelChanger.GetComponent<Animator>();
                    yield return StartCoroutine(transition.PlayFadeOutFast());
                    playerManager.inCombat = false;
                    SceneManager.LoadScene("Scenes/DungeonMap");
                }
                else 
                {
                    musicManager.PlayDenied();
                    UIBlocker.SetActive(true);
                    Debug.Log("Enemy was too fast, player failed to flee the encounter");
                    yield return StartCoroutine(displayAction("Failed to run!"));
                    playerMove = false;
                }
            }
        }
    }

    public void playerCast() {
        if (playerMove && spells.Count > 0) {
            musicManager.PlayConfirmed();
            displaySpellButtons();
        } else {
            musicManager.PlayDenied();
            StartCoroutine(displayAction("No Spells Available!"));
        }
    }

    private IEnumerator displayAction(string text) {
        Debug.Log("BATTLE TEXT DISPLAYED");
        battleTextPanel.SetActive(true);
        battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(1.5f);
        battleTextPanel.SetActive(false);

        yield break;
    }

    // Coroutine so it waits for minigame to finish before moving on
    private IEnumerator HandlePlayerCast() {
        // Wait for the minigame to finish
        UIBlocker.SetActive(true);

        if (minigame != null ) {
            yield return StartCoroutine(minigame.StartMinigame());
        }
        

        // Check the result of the minigame
        minigameSuccess = minigame.isMinigameSuccessful;

        if (minigameSuccess) {
                usedItem.actionType = ActionType.Cast;

                battle.perform(BattleOption.USE_ITEM);   
                playerEntity.remainingMP -= usedItem.manaCost;

                StartCoroutine(displayAction(usedItem.onUseText));

                if (spellAnimationPlayer != null) {
                    spellAnimationPlayer.damage = battle.dmgDealt;
                    yield return StartCoroutine(spellAnimationPlayer.StartAnimation());
                } else {
                    recalculateEnemyHealthBar();
                    updatePlayerHealthAndManaBar();
                    updatePlayerHealthAndManaText();
                }
                
                checkEnemyDeath();

                spellAnimationPlayer = null;
        } else {
            battle.endTurn();
            playerEntity.remainingMP -= usedItem.manaCost;
            updatePlayerHealthAndManaBar();
            updatePlayerHealthAndManaText();
        }

        playerMove = false;
        UIBlocker.SetActive(false);

        minigameSuccess = false;
        if (minigame != null) {
            Destroy(minigame.gameObject);
        }
        

        yield break;
    }

    public void openInventory() {
        if(playerMove && consumables.Count > 0) {
            musicManager.PlayConfirmed();
            displayConsumableButtons();
        } else {
            musicManager.PlayDenied();
            StartCoroutine(displayAction("No Consumables in Inventory!"));
        }
    }
    
    
//Enemy Action
    public void improvedEnemyAI() {
        //Step 1: Get the best source of damage and best source of healing available
        Item [] items = GetComponentInParent<AvailableItemsAccess>().availableItems;
        Item bestDamage, bestHealing;
        float maxDamageOutput, maxHealing;
        int idx = 0, numItems = 1;
        string text = "Enemy attacks!";
        //Initialize variables to value of equipped main hand or unarmed attack
        if(enemyEntity.equippedGear[1] == null){
            bestDamage = items[9];
            bestHealing = items[9];
            battle.setUsedItem(bestDamage);
            maxDamageOutput = battle.returnDamage();
            maxHealing = items[9].healing;
        } else {
            bestDamage = enemyEntity.equippedGear[1].itemData;
            bestHealing = enemyEntity.equippedGear[1].itemData;
            battle.setUsedItem(bestDamage);
            maxDamageOutput = battle.returnDamage();
            maxHealing = bestDamage.healing;
        }
        //Get best items in my inventory
        for(int i =0; i < enemyEntity.inventory.Length; i++){
            if(enemyEntity.inventory[i] != null && enemyEntity.inventory[i].itemData != null){
                numItems++;
                Item inventoryItem = enemyEntity.inventory[i].itemData;
                Debug.Log("Checking "+inventoryItem.name);
                //If it is either not a spell or I have enough MP to cast it, I can check it out.
                if(inventoryItem.actionType != ActionType.Cast || inventoryItem.manaCost <= enemyEntity.remainingMP){
                    battle.setUsedItem(inventoryItem);
                    if(battle.returnDamage() > maxDamageOutput){
                        bestDamage = inventoryItem;
                        maxDamageOutput = battle.returnDamage();
                    }
                    if(inventoryItem.healing > maxHealing){
                        idx = i;
                        bestHealing = inventoryItem;
                        maxHealing = inventoryItem.healing;
                    }
                }
            }
        }
        //By this point, I have obtained the best items to be used, so I need to make the decision
        int rand_num = UnityEngine.Random.Range(1,11);
        if(rand_num > player.level){ //As the player gets more powerful, the enemy is less likely to make mistakes
            Debug.Log("Taking Random Action");
            rand_num = UnityEngine.Random.Range(0,numItems);
            
            Debug.Log("Item Selected is "+rand_num);
            if(rand_num == numItems-1){
                battle.setUsedItem(enemyEntity.equippedGear[1].itemData);
            } else {
                battle.setUsedItem(enemyEntity.inventory[rand_num].itemData);
                if(enemyEntity.inventory[rand_num].itemData.actionType == ActionType.Consume){
                    text = "Enemy used a potion!";
                    enemyEntity.inventory[rand_num].count -= 1;
                    if(enemyEntity.inventory[rand_num].count == 0){
                        enemyEntity.inventory[rand_num] = null;
                    }
                }
            }
        } else {
            if(maxDamageOutput >= playerEntity.remainingHP){
                battle.setUsedItem(bestDamage);
            } else if (enemyEntity.remainingHP/enemyEntity.remainingHP <= 0.25f && maxHealing > 0){
                enemyEntity.inventory[idx].count -= 1;
                if(enemyEntity.inventory[idx].count == 0){
                    enemyEntity.inventory[idx] = null;
                }
                battle.setUsedItem(bestHealing);
                text = "Enemy used a potion!";
            } else {
                battle.setUsedItem(bestDamage);
            }
        }
        battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        if(battle.usedItem.actionType == ActionType.Cast){
            //Has a successful cast chance equal to 10*player level %
            if(UnityEngine.Random.Range(1,11) <= player.level){
                battle.perform(BattleOption.USE_ITEM);
            } else {
                battle.endTurn();
            }
        } else {
            battle.perform(BattleOption.USE_ITEM);
        }
        updatePlayerHealthAndManaBar();
        recalculateEnemyHealthBar(); 
        updatePlayerHealthAndManaText();
    }


    void displayConsumableButtons() {
        foreach (Transform child in spellListContainer)
        {
            Destroy(child.gameObject); // Clear existing buttons
        }

        foreach(ItemSave consumable in consumables) {
            Item item = consumable.itemData;

            GameObject buttonObj = Instantiate(inventoryButtonPrefab, spellListContainer);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{item.name}: x{consumable.count}";

            HoverButton hoverItemButton = buttonObj.GetComponent<HoverButton>();
            hoverItemButton.item = item;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (playerMove) {
                    StartCoroutine(handlePlayerConsume(consumable, item, buttonText));
                }
             });
            
        }
    }

    IEnumerator handlePlayerConsume(ItemSave consumable, Item item, TMP_Text buttonText) {
        if (currentItemInfo != null) {
            Destroy(currentItemInfo.gameObject);
        }

        usedItem = item;
        usedItem.actionType = ActionType.Consume;
        battle.setUsedItem(usedItem);

        if (item.healing > 0 && playerEntity.remainingHP < playerEntity.maximumHP || 
            item.manaRestore > 0 && playerEntity.remainingMP < playerEntity.maximumMP) {

            musicManager.PlayUse();
            battle.perform(BattleOption.USE_ITEM);

            Debug.Log(usedItem.onUseText);

            updatePlayerHealthAndManaText();

            updatePlayerHealthAndManaBar();

            consumable.count -= 1;

            yield return StartCoroutine(displayAction(usedItem.onUseText));

            if (consumable.count <= 0) {
                consumables.Remove(consumable);
                displayConsumableButtons();
            }
            buttonText.text = $"{item.name}: x{consumable.count}";
            playerMove = false;
        }  else {
            musicManager.PlayDenied();
        }
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

            GameObject buttonObj = Instantiate(inventoryButtonPrefab, spellListContainer);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = item.name;

            HoverButton hoverSpellButton = buttonObj.GetComponent<HoverButton>();
            hoverSpellButton.item = item;

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (playerMove && playerEntity.remainingMP >= item.manaCost) {
                    buttonObj.GetComponent<AudioManager>().PlayConfirmed();
                    if (currentItemInfo != null) {
                        Destroy(currentItemInfo.gameObject);
                    }
                    usedItem = item;
                    battle.setUsedItem(usedItem);

                    OpenMinigame minigameOpenerInstance = Instantiate(item.minigameOpener);
                    
                    if (minigame != null) {
                        Destroy(minigame);
                    }
                    minigame = minigameOpenerInstance;
                    minigame.minigamePrefab = item.minigame;
                    minigame.canvas = battleCanvas;

                    if (item.spellAnimationPrefab != null) {
                        spellAnimationPlayer = item.spellAnimationPrefab;
                        spellAnimationPlayer.playerPosition = playerEntity.transform;
                        spellAnimationPlayer.enemyPostion = enemyEntity.transform;
                        spellAnimationPlayer.damagePopupGenerator = popupGenerator;
                    }
                    
                    minigame.gameObject.SetActive(true);

                    StartCoroutine(HandlePlayerCast());

                } else {
                    buttonObj.GetComponent<AudioManager>().PlayDenied();
                }
                
            });
        }

    }

    public void displayItemInformation(string itemName, string itemDescription, Vector2 buttonPos) {
        if (currentItemInfo != null) {
            Destroy(currentItemInfo.gameObject);
        }

        currentItemInfo = Instantiate(inventoryInfoPrefab, inventoryPopupContainer);
        currentItemInfo.GetComponent<PopupInfo>().Setup(itemName, itemDescription);
    }

    public void DestroyItemInfo()
    {
        if(currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }
    }

}
