using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    bool hasCheckedDeath = false;
    public int currentLevel = 0;
    public List<GameObject> orcs = new List<GameObject>();
    [HideInInspector] public GameObject inventoryGameObject;
    [HideInInspector] public GameObject escapeGameObject;
    [HideInInspector] public GameObject upgradeGameObject;
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
        upgradeGameObject = GameObject.FindGameObjectWithTag("UpgradeMenu");
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
        upgradeGameObject = null;
        levelChangerGameObject = null;
        dialogueGameObject = null;
        upgradeMenu = null;

        defeatedEnemies.Clear();
        playerCanCollide = true;
        isMenuActive = false;
        isNewPlayer = true;

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
 
         if (inventoryGameObject != null && escapeGameObject != null && levelChangerGameObject != null) {
            isMenuActive = (dialogueGameObject != null && dialogueGameObject.activeSelf) || (upgradeMenu != null && upgradeMenu.activeSelf) || inventoryGameObject.activeSelf || escapeGameObject.activeSelf || levelChangerGameObject.GetComponent<FadeTransition>().isFadingOut || playerEntity.remainingHP <= 0;
         } else {
            isMenuActive = false;
         }
         
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        if (scene.name == "DungeonMap")
        {
            inventoryGameObject = GameObject.FindGameObjectWithTag("InventoryMenu");
            escapeGameObject = GameObject.FindGameObjectWithTag("EscapeMenu");
            levelChangerGameObject = GameObject.FindGameObjectWithTag("LevelChanger");
            upgradeGameObject = GameObject.FindGameObjectWithTag("UpgradeMenu");
            deathMenuManager = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<DeathMenuManager>();
            this.playerCanCollide = true;
        } else if (scene.name == "CombatManagerScene") {
            Debug.Log("------------------WE IN COMBAT MANAGER!----------------------");
        }
    }

    public void getEnemyEntity() {
        Vector3 spawnPosition = new Vector3(3f, 0.85f, -1f);
        GameObject spawned = Instantiate(orcs[0], spawnPosition, Quaternion.identity);
        spawned.transform.rotation = Quaternion.Euler(0f, -130f, 0f);
        spawned.transform.localScale = new Vector3(1.1f, 1.3f, 1.1f);


        BattleManager bm = BattleManager.instance;
        bm.enemyGameObject = spawned;
        bm.enemyEntity = spawned.GetComponent<Entity>();
        Debug.Log($"SPAWED HP: {bm.enemyEntity.remainingHP}");
        bm.enemy = bm.enemyEntity.getAdjustedStats();
        //Debug.Log($"AFTER SPAWNED: {bm.enemy.health}");
        //Debug.Log(bm.enemy.health);
        
        bm.battle = new Battle(bm.playerEntity, bm.enemyEntity, bm.usedItem, bm.popupGenerator);
        // Debug.Log("BATTLE STARTED!\n"+"Enemy HP: " + bm.enemyEntity.remainingHP + "/" + bm.enemy.health+" - Player HP: "+ bm.playerEntity.remainingHP +"/"+ bm.player.health);
        bm.playerMove = bm.player.speed >= bm.enemy.speed;

        Transform hpImageTransform = spawned.transform.Find("EnemyHealthCanvas");
        Transform hpImageContainerTransform = hpImageTransform.transform.Find("Enemy_HP");

        // Rotate the health canvas to face the camera and add a 40-degree tilt
        hpImageTransform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        Image hpImage = hpImageContainerTransform.GetComponent<Image>();

        bm.enemyHealthBar = hpImage;
        

        GameObject placeholder = GameObject.FindGameObjectWithTag("enemy_to_destroy");
        if (placeholder != null) {
            Destroy(placeholder);
        }

        bm.isEnemyReady = true;
    }

    public IEnumerator DelayedDungeonRestore()
    {
        yield return null;
        yield return null;

        Debug.Log("WE ARE BACK IN DUNGEON FROM THE BATTLE SCENE");

        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
        inventoryManager.UpdateInventoryUIWithItemSave();
        this.playerCanCollide = true;
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
