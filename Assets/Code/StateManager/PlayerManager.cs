using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager player;

    public Entity playerEntity;
    public Item[] itemsArray;
    public int enemyBeforeCombat;
    public Vector3 enemyPositionBeforeCombat;
    public bool isMenuActive = false; 
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
        deathMenuManager = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<DeathMenuManager>();
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
            this.playerCanCollide = true;
        }
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
        if (playerEntity.remainingHP <= 0) {
            StartCoroutine(deathMenuManager.Setup());
        }
    }

    public Entity entity() {
        return playerEntity;
    }
}
