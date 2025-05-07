using System;
using NUnit.Framework.Internal;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject inventoryGroup;

    public int maxStackedItems = 4;

    [SerializeField] public int selectedSlot = -1;

    [SerializeField] GameObject swordPrefab;
    [SerializeField] GameObject runeSwordPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] Material leatherMaterial;
    [SerializeField] Material metalMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject helpPanel;
    [HideInInspector] Entity playerEntity;
    [HideInInspector] PlayerManager playerManager;
    [HideInInspector] public GameObject inventoryContainer;
    [HideInInspector] public GameObject hotbarContainer;
    [HideInInspector] public GameObject equippedContainer;
    [HideInInspector] public GameObject musicManager;
    // random items for testing
    private ItemSave[] itemsInInventory = new ItemSave[25];
    public Item[] itemsArray;

    Item[] itemsToPickup;
    
    public bool equipped = false;
    void Awake()
    {
        if (playerEntity == null) {
            playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
        }
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        inventoryContainer = GameObject.FindGameObjectWithTag("gui_inventory");
        hotbarContainer = GameObject.FindGameObjectWithTag("gui_hotbar");
        equippedContainer = GameObject.FindGameObjectWithTag("gui_equipment");
        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
        helpPanel = GameObject.FindGameObjectWithTag("HelpMenu");

        itemsToPickup = GetComponentInParent<AvailableItemsAccess>().availableItems;
    }

    private void Start() {
        inventoryGroup.SetActive(false);
        helpPanel.SetActive(false);
        ChangeSelectedSlot(0);
    }

    public void UpdateInventoryUIWithItemSave() {
        playerEntity.inventoryCount = 0;

        for (int i = 0; i < 7; i++) 
        { 
            if (playerEntity.inventory[playerEntity.inventoryCount] != null && playerEntity.inventory[playerEntity.inventoryCount].itemData != null && playerEntity.inventory[playerEntity.inventoryCount].count > 0) {
                Transform child = hotbarContainer.transform.GetChild(i);
                InventoryItem item = SpawnNewItemForSave(playerEntity.inventory[playerEntity.inventoryCount].count, playerEntity.inventory[playerEntity.inventoryCount].itemData, child.GetComponent<InventorySlot>());
            }
            playerEntity.inventoryCount++;
        }

        for (int i = 0; i < 18; i++) 
        { 
            if (playerEntity.inventory[playerEntity.inventoryCount] != null && playerEntity.inventory[playerEntity.inventoryCount].itemData != null && playerEntity.inventory[playerEntity.inventoryCount].count > 0) {
                Transform child = inventoryContainer.transform.GetChild(i);
                InventoryItem item = SpawnNewItemForSave(playerEntity.inventory[playerEntity.inventoryCount].count, playerEntity.inventory[playerEntity.inventoryCount].itemData, child.GetComponent<InventorySlot>());
            }
            playerEntity.inventoryCount++;
        }

        for(int i = 0; i < 3; i++) 
        {
            if(playerEntity.equippedGear[i] != null && playerEntity.equippedGear[i].itemData != null) {
                Transform child = equippedContainer.transform.GetChild(i);
                InventoryItem item = SpawnNewItemForSave(1,playerEntity.equippedGear[i].itemData,child.GetComponent<InventorySlot>());
            }
        }
    }

    InventoryItem SpawnNewItemForSave(int n, Item item, InventorySlot slot) {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        inventoryItem.count = n;
        inventoryItem.countText.text = inventoryItem.count.ToString();
        inventoryItem.countText.gameObject.SetActive(inventoryItem.count > 1);
        inventoryItem.RefreshCount();
        Debug.Log("SPAWNED THE N GIVEN WAS " + n + " AND THE ITEM N IS " + inventoryItem.count );
        
        return inventoryItem;
    }

    public void SendCurrentInventoryToState() {
        ClearInventoryUI();

        Debug.Log("------ HERE ARE THE SLOTS IN THE HOTBAR WHEN CALLED ------");
        if (playerEntity == null) {
            playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
        }
        playerEntity.inventoryCount = 0;    

        for (int i = 0; i < 7; i++) 
        { 
            Transform child = hotbarContainer.transform.GetChild(i); 
            if (child.childCount > 0)
            {
                Transform grandchild = child.GetChild(0); 
                InventoryItem item = grandchild.GetComponent<InventoryItem>();
                if (item != null)
                {
                    Debug.Log(item.name + " - " + item.item.name+ " X" + item.count);

                    ItemSave itemSave = new ItemSave();
                    itemSave.count = item.count;
                    itemSave.item = item.item.name;
                    itemSave.itemData = item.item;
                    
                    playerEntity.inventory[playerEntity.inventoryCount] = itemSave;
                    if (itemSave.count == 0) {
                        playerEntity.inventory[playerEntity.inventoryCount] = null;
                    }
                }
            } else {
                playerEntity.inventory[playerEntity.inventoryCount] = null;
            }
            playerEntity.inventoryCount++;
        }

        Debug.Log("-------------------------------------------------");
        Debug.Log("------ HERE ARE THE SLOTS IN THE INVENTORY WHEN CALLED ------");

        for (int i = 0; i < 18; i++) 
        { 
            Transform child = inventoryContainer.transform.GetChild(i); 
            if (child.childCount > 0)
            {
                Transform grandchild = child.GetChild(0); 
                InventoryItem item = grandchild.GetComponent<InventoryItem>();
                if (item != null)
                {
                    Debug.Log(item.name + " - " + item.item.name);

                    ItemSave itemSave = new ItemSave();
                    itemSave.count = item.count;
                    itemSave.item = item.item.name;
                    itemSave.itemData = item.item;

                    playerEntity.inventory[playerEntity.inventoryCount] = itemSave;
                    if (itemSave.count == 0) {
                        playerEntity.inventory[playerEntity.inventoryCount] = null;
                    }
                }
            } else {
                playerEntity.inventory[playerEntity.inventoryCount] = null;
            }
            playerEntity.inventoryCount++;
        }

        Debug.Log("-------------------------------------------------");
        Debug.Log("------ HERE ARE THE EQUIPPED ITEMS WHEN CALLED ------");

        for(int i = 0; i < 3; i++)
        {
            Transform child = equippedContainer.transform.GetChild(i);
            if(child.childCount > 0) 
            {
                Transform grandchild = child.GetChild(0); 
                InventoryItem item = grandchild.GetComponent<InventoryItem>();
                if(item != null) {
                    Debug.Log(item.name + " - " + item.item.name);

                    ItemSave itemSave = new ItemSave();
                    itemSave.count = item.count;
                    itemSave.item = item.item.name;
                    itemSave.itemData = item.item;

                    playerEntity.equippedGear[i] = itemSave;
                    if (itemSave.count == 0) {
                        playerEntity.equippedGear[i] = null;
                    }
                }
            } else {
                playerEntity.equippedGear[i] = null;
            }
        }
    }
    
    public void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        RemoveAllChildrenFromTorso();
    }


    // TEST SUITE FOR ADDING ITEMS TO INVENTORY LOGIC

    private void Update()
    {
        // OPEN INVENTORY
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryGroup.SetActive(!inventoryGroup.activeSelf);
            DestroyAllPopupPanels();
        }

        // TEST SUITE
        if (Input.GetKeyDown(KeyCode.O))
        {
            PickupItem();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            CreateSpell();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateManaPotion();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateHealthPotion();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateShield();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Item item = UseSelectedItem();
            if (item != null) {
                Debug.Log($"Selected item: {item.name}");
            } else {
                Debug.Log("No item selected!");
            }
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            helpPanel.SetActive(!helpPanel.activeSelf);
            DestroyAllPopupPanels();
        }

        // HOTBAR SWITCHING
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSelectedSlot(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSelectedSlot(1);

        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSelectedSlot(2);

        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSelectedSlot(3);

        } else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSelectedSlot(4);

        } else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSelectedSlot(5);

        } else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeSelectedSlot(6);

        }

        // if (Input.GetKeyDown(KeyCode.E)) {
        //     SendCurrentInventoryToState();
        // }

        // SHOW WEAPON ON CHARACTER IF ITS CURRENTLY SELECTED
        //Step 1: Get the torso
        GameObject torso = GameObject.FindGameObjectWithTag("Torso");
        if(torso != null) {
            //Step 2: Get the currently equipped weapon
            Transform child = equippedContainer.transform.GetChild(1);
            if(child.childCount > 0) {
                Transform grandChild = child.GetChild(0);
                InventoryItem item = grandChild.GetComponent<InventoryItem>();
                if(item != null && !equipped) {
                    GameObject newSword;
                    if (item.item.name.Contains("Rune Sword")) {
                        newSword = Instantiate(runeSwordPrefab, torso.transform);
                        newSword.transform.localPosition = new Vector3(0.01076f, -0.01143f, 0.03788f);
                        newSword.transform.localEulerAngles = new Vector3(-60f, 0f, -90f);
                        newSword.transform.localScale = new Vector3(0.01598134f, 0.01902541f, 0.01598134f);
                        Debug.Log("Rune sword attached.");
                    } else if (item.item.name.Contains("Basic Sword")) {

                        newSword = Instantiate(swordPrefab, torso.transform);
                        newSword.transform.localPosition = new Vector3(0.0073f, 0f, 0.0143f);
                        newSword.transform.localEulerAngles = new Vector3(-60f, 0f, -90f);
                        newSword.transform.localScale = new Vector3(0.01598134f, 0.01902541f, 0.01598134f);
                        Debug.Log("Basic sword attached.");
                    } else {
                        Debug.LogWarning("What kind of sword is this???");
                        return;
                    }
                    equipped = true;
                    Debug.Log("Sword equipped.");
                } 
            }
            //Step 3: Get the currently equipped Shield
            child = equippedContainer.transform.GetChild(2);
            if(child.childCount > 0) {
                Transform grandChild = child.GetChild(0);
                InventoryItem item = grandChild.GetComponent<InventoryItem>();
                if(item != null) {
                    GameObject shield = Instantiate(shieldPrefab, torso.transform);
                    shield.transform.localPosition = new Vector3(0.0126599995f,0,0.0137999998f);
                    shield.transform.localEulerAngles = new Vector3(0,90f,270f);
                    shield.transform.localScale = new Vector3(0.015f,0.015f,0.015f);
                    Debug.Log("Shield attached");
                }
            }
            //Step 4: Change shirt color
            child = equippedContainer.transform.GetChild(0);
            if(child.childCount > 0) {
                Transform grandChild = child.GetChild(0);
                InventoryItem item = grandChild.GetComponent<InventoryItem>();
                if(item != null) {
                    if(item.item.name.Contains("Leather Armor")) {
                        Debug.Log("LeatherArmor Equipped");
                        torso.GetComponent<MeshRenderer>().material = leatherMaterial;
                    } else if (item.item.name.Contains("Chain Mail")) {
                        torso.GetComponent<MeshRenderer>().material = metalMaterial;
                    } else {
                        torso.GetComponent<MeshRenderer>().material = defaultMaterial;
                    }
                } else {
                    torso.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
            } else {
                torso.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
        } else {
            Debug.LogWarning("No torso found in the scene..... for some reason...");
        }     
    }

    public void RemoveAllChildrenFromTorso() {
        GameObject torso = GameObject.FindGameObjectWithTag("Torso");
        equipped = false;

        if (torso != null) {
            foreach (Transform child in torso.transform) {
                Destroy(child.gameObject);
            }
            //Debug.Log("All children removed from Torso.");
        } else {
            Debug.LogWarning("No torso found in the scene..... for some reason...");
        }
    }


    public bool AddItem(Item item) {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems && item.stackable == true) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }


        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                InventoryItem spawnedItem = SpawnNewItem(item, slot);
                return true;
            }
        }

        SendCurrentInventoryToState();
        return false;
    }

    // void AddItemToPlayerInventory(InventoryItem inItem) {
    //     if (playerEntity) {
    //         Debug.Log("----- IMPORTANT -------> " + inItem.item.name);
    //         for (int i = 0; i < playerEntity.inventory.Count; i++) {
    //             Debug.Log("inventory currently: " + i);
    //             if (playerEntity.inventory[i] == null) {
    //                 playerEntity.inventory[i] = inItem.item;
    //                 playerEntity.inventoryCount++;
    //                 Debug.Log("Added item to player inventory.");
    //                 break;
    //             }
    //         }

    //         for (int i = 0; i < playerEntity.equippedGear.Length; i++) {
    //             Debug.Log("equippedGear currently: " + i);
    //             if (playerEntity.equippedGear[i] == null) {
    //                 playerEntity.equippedGear[i] = inItem.item;
    //                 playerEntity.equippedGearCount++;
    //                 Debug.Log("Added item to player equipped gear.");
    //                 break;
    //             }
    //         }

    //         Debug.Log("Player entity found. Adding item to inventory.");
    //     } else {
    //         Debug.LogWarning("No player entity found. Can't add item to inventory.");
    //     }
    // }

    InventoryItem SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        return inventoryItem;
    }

    public void PickupItem() {
        int id = UnityEngine.Random.Range(0, itemsToPickup.Length);
        bool res = AddItem(itemsToPickup[id]);
        if (res) {
            Debug.Log($"Picked up {itemsToPickup[id].name}");
        } else {
            Debug.Log("Inventory is full!");
        }
        SendCurrentInventoryToState();
    }

    //This creates an issue
    public void CreateSpell() {
        int id = UnityEngine.Random.Range(5,9);
        bool res = AddItem(itemsToPickup[id]);
        if (res) {
            Debug.Log($"Picked up {itemsToPickup[id].name}");
        } else {
            Debug.Log("Inventory is full!");
        }
        SendCurrentInventoryToState();
    }

    public void CreateHealthPotion() {
        int id = 1;
        bool res = AddItem(itemsToPickup[id]);
        if (res) {
            Debug.Log($"Picked up {itemsToPickup[id].name}");
        } else {
            Debug.Log("Inventory is full!");
        }
        SendCurrentInventoryToState();
    }
    public void CreateManaPotion() {
        int id = 2;
        bool res = AddItem(itemsToPickup[id]);
        if (res) {
            Debug.Log($"Picked up {itemsToPickup[id].name}");
        } else {
            Debug.Log("Inventory is full!");
        }
        SendCurrentInventoryToState();
    }
    public void CreateShield() {
        int id = 10;
        bool res = AddItem(itemsToPickup[id]);
        if (res) {
            Debug.Log($"Picked up {itemsToPickup[id].name}");
        } else {
            Debug.Log("Inventory is full!");
        }
        SendCurrentInventoryToState();
    }

    public Item UseSelectedItem() {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) {
            Item item = itemInSlot.item;
            if (item.consumable == true) {
                itemInSlot.count--;
                musicManager.GetComponent<AudioManager>().PlayUse();
                if (itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }

                playerEntity.remainingHP += item.healing;
                playerEntity.remainingMP += item.manaRestore;
                recalculatePlayerHealthAndMana();
                
                SendCurrentInventoryToState();
            }
            return item;
        } else {
            return null;
        }
    }

    private void recalculatePlayerHealthAndMana() {
        playerEntity.getAdjustedStats();
        playerEntity.remainingHP = Mathf.Clamp(playerEntity.remainingHP, 0, playerEntity.maximumHP);
        playerEntity.remainingMP = Mathf.Clamp(playerEntity.remainingMP, 0, playerEntity.maximumMP);
    }

    private void DestroyAllPopupPanels()
    {
        // Find all active popup panels in the scene
        InventoryItem[] inventoryItems = FindObjectsByType<InventoryItem>(FindObjectsSortMode.None);

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.currentPopupPanel != null)
            {
                Destroy(item.currentPopupPanel);
                item.currentPopupPanel = null; // Clear the reference
            }
        }
    }

    // Clears the inventory UI so new items can be instantiated
    private void ClearInventoryUI()
    {
        foreach (Transform child in hotbarContainer.transform)
        {
            foreach (Transform grandchild in child)
            {
                Destroy(grandchild.gameObject);
            }
        }

        foreach (Transform child in inventoryContainer.transform)
        {
            foreach (Transform grandchild in child)
            {
                Destroy(grandchild.gameObject);
            }
        }

        foreach (Transform child in equippedContainer.transform)
        {
            foreach (Transform grandchild in child)
            {
                Destroy(grandchild.gameObject);
            }
        }
    }
}