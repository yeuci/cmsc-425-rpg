using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager player;

    public Entity playerEntity;
    public Item[] itemsArray;
    public int enemyBeforeCombat;
    public Vector3 enemyPositionBeforeCombat;
    public bool isMenuActive = false;
    public bool isNewPlayer = true; 
    public bool inCombat;
    public bool hasCheckedDeath = false;
    public int currentLevel = 0;
    public int currentDungeonLevel = 0;
    public List<GameObject> orcs = new List<GameObject>();
    [HideInInspector] public GameObject inventoryGameObject;
    [HideInInspector] public GameObject escapeGameObject;
    [HideInInspector]  GameObject levelChangerGameObject;
    [HideInInspector] public GameObject dialogueGameObject;
    [HideInInspector] public GameObject upgradeMenu;
    [HideInInspector] DeathMenuManager deathMenuManager;


    public List<int> defeatedEnemies = new List<int>();
    public bool playerCanCollide = true;

    void Awake()
    {
        if(player != null) {
            Destroy(gameObject);
            return;
        }   

        Debug.Log("MADE");
        player = this;
        playerEntity = player.AddComponent<Entity>();
        DontDestroyOnLoad(gameObject);
        inventoryGameObject = GameObject.FindGameObjectWithTag("InventoryMenu");
        escapeGameObject = GameObject.FindGameObjectWithTag("EscapeMenu");
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        levelChangerGameObject = GameObject.FindGameObjectWithTag("LevelChanger");
        dialogueGameObject = GameObject.FindGameObjectWithTag("dialogue_container_inner");
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        deathMenuManager = GameObject.FindGameObjectWithTag("DeathMenu")?.GetComponent<DeathMenuManager>();
    }

    public void ResetPlayerManager()
    {
        // if (playerEntity != null)
        // {
        //     Destroy(playerEntity);
        // }
        // playerEntity = player.AddComponent<Entity>();

        // // itemsArray = new Item[0];

        // enemyBeforeCombat = 0;
        // enemyPositionBeforeCombat = Vector3.zero;

        // inventoryGameObject = null;
        // escapeGameObject = null;
        // levelChangerGameObject = null;
        // dialogueGameObject = null;
        // upgradeMenu = null;

        // defeatedEnemies.Clear();
        // playerCanCollide = true;
        // isMenuActive = false;
        // isNewPlayer = true;

        if (playerEntity != null)
        {
            Destroy(playerEntity);
        }
        playerEntity = player.AddComponent<Entity>();

        // itemsArray = new Item[0];
        enemyBeforeCombat = 0;
        enemyPositionBeforeCombat = Vector3.zero;

        inventoryGameObject = null;
        escapeGameObject = null;
        levelChangerGameObject = null;
        dialogueGameObject = null;
        upgradeMenu = null;

        currentLevel = 0;
        currentDungeonLevel = 0;
        inCombat = false;
        hasCheckedDeath = false;
        isMenuActive = false;
        isNewPlayer = true;
        playerCanCollide = true;

        defeatedEnemies.Clear();
        deathMenuManager = null;

        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        deathMenuManager = GameObject.FindGameObjectWithTag("DeathMenu")?.GetComponent<DeathMenuManager>();

        Debug.Log("PlayerManager has been reset.");
    }



    // Update is called once per frame
    void Update()
    {   
         if (dialogueGameObject == null) {
            dialogueGameObject = GameObject.FindGameObjectWithTag("dialogue_container_inner");
         }

        //  if (upgradeMenu == null) {
        //     upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
        //  }
        checkDeath();
 
         if (inventoryGameObject != null && escapeGameObject != null && levelChangerGameObject != null && upgradeMenu != null) {
            isMenuActive = (dialogueGameObject != null && dialogueGameObject.activeSelf) || (upgradeMenu != null && upgradeMenu.activeSelf) || inventoryGameObject.activeSelf || escapeGameObject.activeSelf || levelChangerGameObject.GetComponent<FadeTransition>().isFadingOut ||
            playerEntity.remainingHP <= 0;
         } else {
            isMenuActive = false;
         }


         
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        if (scene.name == "DungeonMap")
        {
            InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
            inventoryGameObject = GameObject.FindGameObjectWithTag("InventoryMenu");
            escapeGameObject = GameObject.FindGameObjectWithTag("EscapeMenu");
            levelChangerGameObject = GameObject.FindGameObjectWithTag("LevelChanger");
            upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu");
            deathMenuManager = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<DeathMenuManager>();
            deathMenuManager.deathMenu.SetActive(false);
            this.playerCanCollide = true;

            dungeonSwitcher();

            int destroyed = 0;
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag("Enemy"))
                {
                    Entity entity = obj.GetComponent<Entity>();
                    if (entity != null)
                    {
                        Debug.Log($"Checking enemy: {entity.enemyId} against defeated list.");
                        Debug.Log(this.defeatedEnemies.Count);
                        Debug.Log(this.defeatedEnemies);
                        if (this.defeatedEnemies.Contains(entity.enemyId)) {
                            obj.gameObject.SetActive(false);
                            Destroy(obj);
                            destroyed += 1;
                        }
                    } else {
                        Debug.LogWarning("Entity component not found on enemy object!");
                    }
                }
            }

            inventoryManager.UpdateInventoryUIWithItemSave();
        } else if (scene.name == "CombatManagerScene") {
            Debug.Log("------------------WE IN COMBAT MANAGER!----------------------");
        }
    }

    void dungeonSwitcher() {
            GameObject dungeonOne = GameObject.FindGameObjectWithTag("Dungeon1");
            GameObject dungeonTwo = GameObject.FindGameObjectWithTag("Dungeon2");
            GameObject dungeonThree = GameObject.FindGameObjectWithTag("Dungeon3");

            if (currentDungeonLevel == 0) {
                Debug.Log("-----------------DUNGEON ONE ACTIVE-----------------");

                dungeonOne.SetActive(true);
                dungeonTwo.SetActive(false);
                dungeonThree.SetActive(false);
            } else if (currentDungeonLevel == 1) {
                Debug.Log("-----------------DUNGEON TWO ACTIVE-----------------");
                
                dungeonOne.SetActive(false);
                dungeonTwo.SetActive(true);
                dungeonThree.SetActive(false);
            } else if (currentDungeonLevel == 2) {
                Debug.Log("-----------------DUNGEON THREE ACTIVE-----------------");

                dungeonOne.SetActive(false);
                dungeonTwo.SetActive(false);
                dungeonThree.SetActive(true);
            }
    }

    //Note: Creation of Enemy is Working, but I cannot modify stats
    public void getEnemyEntity() {
        Vector3 spawnPosition = new Vector3(3f, 0.85f, -1f);
        GameObject spawned = Instantiate(orcs[currentLevel], spawnPosition, Quaternion.identity);
        spawned.transform.rotation = Quaternion.Euler(0f, -130f, 0f);
        spawned.transform.localScale = new Vector3(1.1f, 1.3f, 1.1f);


        BattleManager bm = BattleManager.instance;
        bm.enemyGameObject = spawned;
        bm.enemyEntity = spawned.GetComponent<Entity>();
        bm.orcModelAnimator = spawned.GetComponent<OrcModelAnimator>();

        Debug.Log($"SPAWED HP: {bm.enemyEntity.remainingHP}");
        // bm.enemyEntity.scaleStats(ScalingMethod.PLAYER_LEVEL);
        // bm.enemy = bm.enemyEntity.getAdjustedStats();
        // //Debug.Log($"AFTER SPAWNED: {bm.enemy.health}");
        // //Debug.Log(bm.enemy.health);
        
        // bm.battle = new Battle(bm.playerEntity, bm.enemyEntity, bm.usedItem, bm.popupGenerator);
        // // Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + bm.enemyEntity.remainingHP + "/" + bm.enemy.health+" - Player HP: "+ bm.playerEntity.remainingHP +"/"+ bm.player.health);
        // bm.playerMove = bm.player.speed >= bm.enemy.speed;

        // Transform hpImageTransform = spawned.transform.Find("EnemyHealthCanvas");
        // Transform hpImageContainerTransform = hpImageTransform.transform.Find("Enemy_HP");

        // // Rotate the health canvas to face the camera and add a 40-degree tilt
        // hpImageTransform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        // Image hpImage = hpImageContainerTransform.GetComponent<Image>();

        // bm.enemyHealthBar = hpImage;
        

        // GameObject placeholder = GameObject.FindGameObjectWithTag("enemy_to_destroy");
        // if (placeholder != null) {
        //     Destroy(placeholder);
        // }

        // bm.isEnemyReady = true;
    }

    public IEnumerator DelayedDungeonRestore()
    {
        yield return null;
        yield return null;

        Debug.Log("WE ARE BACK IN DUNGEON FROM THE BATTLE SCENE");

        GameObject player = GameObject.FindWithTag("Player");
        MonoBehaviour[] playerScripts = player.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != null && (script.GetType().Name.Contains("Movement") || 
                                script.GetType().Name.Contains("Controller") || 
                                script.GetType().Name.Contains("Input") ||
                                script.GetType().Name.Contains("Move"))
                                ) 
            {
                script.enabled = false;
                Debug.Log($"Temporarily disabled: {script.GetType().Name}");
            }
        }
        
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            Debug.Log("Temporarily disabled Character Controller");
        }

        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
        inventoryManager.UpdateInventoryUIWithItemSave();

        player.transform.position = new Vector3(enemyPositionBeforeCombat.x, enemyPositionBeforeCombat.y, enemyPositionBeforeCombat.z);

        Debug.Log("Player position set to 2: " + enemyPositionBeforeCombat);
        Debug.Log("Player position is actually 2 : " + player.transform.position);

        if (controller != null)
        {
            controller.enabled = true;
            Debug.Log("Re-enabled Character Controller");
        }
        
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != null && !script.enabled)
            {
                script.enabled = true;
                Debug.Log($"Re-enabled: {script.GetType().Name}");
            }
        }

        playerCanCollide = true;
    }
    

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void checkDeath() {

        if (playerEntity.remainingHP <= 0 && !inCombat && !hasCheckedDeath) {
            hasCheckedDeath = true;
            StartCoroutine(deathMenuManager.Setup());
        }
    }

    public Entity entity() {
        return playerEntity;
    }
}
