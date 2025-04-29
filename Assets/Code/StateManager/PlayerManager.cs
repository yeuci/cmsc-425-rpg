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
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryGameObject = GameObject.FindGameObjectWithTag("InventoryMenu");
        escapeGameObject = GameObject.FindGameObjectWithTag("EscapeMenu");

        // playerEntity = this.AddComponent<Entity>();
        playerEntity = player.AddComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryGameObject != null && escapeGameObject != null) {
            isMenuActive = inventoryGameObject.activeSelf || escapeGameObject.activeSelf;
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

    public Entity entity() {
        return playerEntity;
    }
}
