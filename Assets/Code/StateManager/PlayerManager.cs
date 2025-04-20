using Unity.VisualScripting;
using UnityEngine;
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
        }
    }

    public Entity entity() {
        return playerEntity;
    }
}
