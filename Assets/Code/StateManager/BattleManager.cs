using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    Entity playerEntity, enemyEntity;                   // Player and enemy entity
    Stat player, enemy;                                 // Player and enemy stats
    Battle battle;                                      // Manages battle actions
    bool playerMove;                                    // Track if player can move
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

        enemyEntity = enemyGameObject.GetComponent<Entity>();
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
        
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        playerEntity = PlayerManager.player.entity();
        playerEntity.transform.position = new Vector3(-3.25f, 0.5f, 0);
        playerEntity.transform.right = Vector3.left;

        getPlayerInventory();
        
        player = playerEntity.getAdjustedStats();
        enemy = enemyEntity.getAdjustedStats();
        
        battle = new Battle(playerEntity, enemyEntity, usedItem, popupGenerator);

        updatePlayerHealthAndManaBar();

        escapeAttempts = 0;

        playerHealthText.text = $"Health: {playerEntity.remainingHP} / {player.health}";
        playerManaText.text = $"Mana: {playerEntity.remainingMP} / {player.mana}";

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
                usedItem = GetComponentInParent<AvailableItemsAccess>().availableItems[8];
            }

            animationManager.Animate(BattleOption.ATTACK);
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
        enemyHealthBar.fillAmount = enemyEntity.remainingHP / enemy.health;
    }

    public void updatePlayerHealthAndManaBar() {
        playerHealthBar.fillAmount = playerEntity.remainingHP / player.health;
        playerManaBar.fillAmount = playerEntity.remainingMP / player.mana;
    }

    public void updatePlayerHealthAndManaText() {
        playerHealthText.text = $"Health: {playerEntity.remainingHP} / {player.health}";
        playerManaText.text = $"Mana: {playerEntity.remainingMP} / {player.mana}";
    }

    void checkEnemyDeath() {
        if(enemyEntity.remainingHP <= 0) {
            float enemyXP = enemyEntity.calculateXPValue();
            Debug.Log("Enemy is defeated. Player gains " + enemyXP + " XP!");

            float prevXP = playerEntity.stats.experience;
            float prevCap = playerEntity.stats.expToNext;
            int prevLvl = playerEntity.stats.level;
            
            playerEntity.stats.experience += enemyXP;
            playerEntity.recalculateLvl();
            
            Debug.Log("Player is Lvl " + playerEntity.stats.level + "! Progress: " + playerEntity.stats.experience + "/"+playerEntity.stats.expToNext);

            musicManager.sceneMusic.Stop();
            musicManager.playVictory();
            UIBlocker.SetActive(true);

            StartCoroutine(results.showVictory(prevLvl, (int)prevXP, (int)prevCap, enemyXP));

            // SceneManager.LoadScene("Scenes/DungeonMap");
        }
    }

    IEnumerator GameOver() {
        yield return new WaitUntil(() => playerMove);
        musicManager.sceneMusic.Stop();
        musicManager.playDefeat();
        gameOverScreen.Setup();
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

                SceneTransition transition = levelChanger.GetComponent<SceneTransition>();
                transition.animator = levelChanger.GetComponent<Animator>();
                yield return StartCoroutine(transition.PlayCombatFinishedTransition());
                SceneManager.LoadScene("Scenes/DungeonMap");
            }
            else {
                escapeAttempts += 1;
                float escapeChance = Mathf.Floor((player.speed * 32) / (enemy.speed / 4)) + 30.0f * escapeAttempts;
                float rand = UnityEngine.Random.Range(0.0f, 255.0f);
                if (rand < escapeChance) 
                {
                    musicManager.PlayConfirmed();
                    Debug.Log("Player has fled the encounter");
                    yield return StartCoroutine(displayAction("Player successfully fled!"));

                    SceneTransition transition = levelChanger.GetComponent<SceneTransition>();
                    transition.animator = levelChanger.GetComponent<Animator>();
                    yield return StartCoroutine(transition.PlayCombatFinishedTransition());
                    SceneManager.LoadScene("Scenes/DungeonMap");
                }
                else 
                {
                    musicManager.PlayDenied();
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

                battleTextPanel.SetActive(true);
                battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = usedItem.onUseText;

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
    //The goal is to have the AI make less mistakes as the player becomes more powerful.
    public void enemyArtificialIntelligence() {
        System.Random rd = new System.Random();
        int rand_num = rd.Next(1,10);
        string text = "";

        if(rand_num <= player.level) {
            //Take a random action. This works if the player can hit level 10.
            Item itemPick = enemyEntity.inventory[rd.Next(0,enemyEntity.inventoryCount)].itemData;
            battle.setUsedItem(itemPick);
        } else {
            Item defaultItem;
            if(enemyEntity.equippedGear[1] == null) {
                defaultItem = GetComponentInParent<AvailableItemsAccess>().availableItems[8];
            } else {
                defaultItem = enemyEntity.equippedGear[1].itemData;
            }
            battle.setUsedItem(defaultItem);

            Item bestDamage = defaultItem;
            Item bestHealing = defaultItem;
            float bestDamageOutput = battle.returnDamage(); //Starts with currently equipped weapon
            float bestHealingAvailable = defaultItem.healing; //Starts with healing of currently equipped weapon
            
            foreach (ItemSave iS in enemyEntity.inventory) {
                if(iS != null && iS.itemData != null) {
                    if(iS.itemData.healing > bestHealingAvailable) {
                        bestHealing = iS.itemData;
                        bestHealingAvailable = iS.itemData.healing;
                    }
                    if(iS.itemData.actionType == ActionType.Attack) { //This should never trigger for an enemy, as they will only have 1 weapon
                        battle.setUsedItem(iS.itemData);
                        if(battle.returnDamage() >= bestDamageOutput) { //Save spell if possible
                            enemyEntity.equippedGear[1] = iS; //Set best damage output to currently equipped weapon
                            bestDamageOutput = battle.returnDamage();
                            bestDamage = iS.itemData;
                        }
                    } else if(iS.itemData.actionType == ActionType.Cast && enemyEntity.remainingMP >= iS.itemData.manaCost) {
                        battle.setUsedItem(iS.itemData);
                        if(battle.returnDamage() > bestDamageOutput) {
                            bestDamageOutput = battle.returnDamage();
                            bestDamage = iS.itemData;
                        }
                        
                    }
                }
            }

            if(playerEntity.remainingHP - bestDamageOutput <= 0) {
                battle.setUsedItem(bestDamage);
                text = "Enemy attacks!";
            } else if (enemyEntity.remainingHP/enemy.health <= 0.25f && bestHealingAvailable > 0) {
                battle.setUsedItem(bestHealing);
                text = "Enemy used a potion!";
            } else {
                battle.setUsedItem(bestDamage);
                text = "Enemy attacks!";
            }

        }

        if (battle.usedItem.actionType == ActionType.Consume) {
                text = "Enemy used a potion!";
            }
        battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        battle.perform(BattleOption.USE_ITEM);
        updatePlayerHealthAndManaBar();
        recalculateEnemyHealthBar(); 
        updatePlayerHealthAndManaText();
    }


    public void improvedEnemyAI() {
        //Step 1: Get the best source of damage and best source of healing available
        Item [] items = GetComponentInParent<AvailableItemsAccess>().availableItems;
        Item bestDamage, bestHealing;
        float maxDamageOutput, maxHealing;
        int idx = 0;
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
        //I now need to loop through my inventory to see if I have a better option.
        /*foreach (ItemSave iS in enemyEntity.inventory){
            if(iS != null & iS.itemData!= null) { //If the item actually exists
                Item inventoryItem = iS.itemData;
                Debug.Log("Checking "+inventoryItem.name);
                //If it is either not a spell or I have enough MP to cast it, I can check it out.
                if(inventoryItem.actionType != ActionType.Cast || inventoryItem.manaCost <= enemyEntity.remainingMP){
                    battle.setUsedItem(inventoryItem);
                    if(battle.returnDamage() > maxDamageOutput){
                        bestDamage = inventoryItem;
                        maxDamageOutput = battle.returnDamage();
                    }
                    if(inventoryItem.healing > maxHealing){
                        bestHealing = inventoryItem;
                        maxHealing = inventoryItem.healing;
                    }
                }
            }
        }*/
        for(int i =0; i < enemyEntity.inventory.Length; i++){
            if(enemyEntity.inventory[i] != null && enemyEntity.inventory[i].itemData != null){
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
        if(maxDamageOutput >= playerEntity.remainingHP){
            battle.setUsedItem(bestDamage);
        } else if (enemyEntity.remainingHP/enemy.health <= 0.25f && maxHealing > 0){
            enemyEntity.inventory[idx].count -= 1;
            if(enemyEntity.inventory[idx].count == 0){
                enemyEntity.inventory[idx] = null;
            }
            battle.setUsedItem(bestHealing);
            text = "Enemy used a potion!";
        } else {
            battle.setUsedItem(bestDamage);
        }
        battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        battle.perform(BattleOption.USE_ITEM);
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
                    if (currentItemInfo != null) {
                        Destroy(currentItemInfo.gameObject);
                    }

                    usedItem = item;
                    usedItem.actionType = ActionType.Consume;
                    battle.setUsedItem(usedItem);

                    if (item.healing > 0 && playerEntity.remainingHP < player.health || 
                        item.manaRestore > 0 && playerEntity.remainingMP < player.mana) {

                        musicManager.PlayUse();
                        battleTextPanel.SetActive(true);
                        battleTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = item.onUseText;
                        battle.perform(BattleOption.USE_ITEM);

                        updatePlayerHealthAndManaText();

                        updatePlayerHealthAndManaBar();

                        consumable.count -= 1;

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
             });
            
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
