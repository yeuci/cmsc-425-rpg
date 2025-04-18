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
    [HideInInspector] Entity playerEntity;
    [HideInInspector] public GameObject inventoryContainer;
    [HideInInspector] public GameObject hotbarContainer;
    // random items for testing
    private ItemSave[] itemsInInventory = new ItemSave[25];
    public Item[] itemsArray;
    
    public bool equipped = false;

    private void Start() {
        playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
        inventoryContainer = GameObject.FindGameObjectWithTag("gui_inventory");
        hotbarContainer = GameObject.FindGameObjectWithTag("gui_hotbar");

        // for (int i = 0; i < itemsInInventory.Length; i++)
        // {
        //     itemsInInventory[i] = new ItemSave();
        //     itemsInInventory[i].count = Random.Range(1, 5);
        //     itemsInInventory[i].item = itemsArray[Random.Range(0, itemsArray.Length)];
        // }

        inventoryGroup.SetActive(false);
        ChangeSelectedSlot(0);
    }

    public void UpdateInventoryUIWithItemSave() {
        playerEntity.inventoryCount = 0;

        for (int i = 0; i < 7; i++) 
        { 
            if (playerEntity.inventory[playerEntity.inventoryCount] != null) {
                Transform child = hotbarContainer.transform.GetChild(i);
                InventoryItem item = SpawnNewItemForSave(playerEntity.inventory[playerEntity.inventoryCount].count, playerEntity.inventory[playerEntity.inventoryCount].itemData, child.GetComponent<InventorySlot>());
            }
            playerEntity.inventoryCount++;
        }

        for (int i = 0; i < 18; i++) 
        { 
            if (playerEntity.inventory[playerEntity.inventoryCount] != null) {
                Transform child = inventoryContainer.transform.GetChild(i);
                InventoryItem item = SpawnNewItemForSave(playerEntity.inventory[playerEntity.inventoryCount].count, playerEntity.inventory[playerEntity.inventoryCount].itemData, child.GetComponent<InventorySlot>());
            }
            playerEntity.inventoryCount++;
        }
    }

    InventoryItem SpawnNewItemForSave(int n, Item item, InventorySlot slot) {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        inventoryItem.count = n;
        inventoryItem.countText.text = inventoryItem.count.ToString();
        inventoryItem.countText.gameObject.SetActive(inventoryItem.count > 1);
        Debug.Log("SPAWNED THE N GIVEN WAS " + n + " AND THE ITEM N IS " + inventoryItem.count );
        return inventoryItem;
    }

    public void SendCurrentInventoryToState() {
        Debug.Log("------ HERE ARE THE SLOTS IN THE HOTBAR WHEN CALLED ------");
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
    public Item[] itemsToPickup;

    private void Update()
    {
        // OPEN INVENTORY
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryGroup.SetActive(!inventoryGroup.activeSelf);
        }

        // TEST SUITE
        if (Input.GetKeyDown(KeyCode.O))
        {
            PickupItem();
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

        if (Input.GetKeyDown(KeyCode.E)) {
            SendCurrentInventoryToState();
        }

        // SHOW WEAPON ON CHARACTER IF ITS CURRENTLY SELECTED
        if (selectedSlot >= 0 && selectedSlot <= 6) {
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot && itemInSlot.item.name.Contains("Sword") && !equipped) {
                GameObject torso = GameObject.FindGameObjectWithTag("Torso");

                if (torso != null) {
                    GameObject newSword;
                    if (itemInSlot.item.name.Contains("RuneSword")) {
                        newSword = Instantiate(runeSwordPrefab, torso.transform);
                        newSword.transform.localPosition = new Vector3(0.01076f, -0.01143f, 0.03788f);
                        newSword.transform.localEulerAngles = new Vector3(-60f, 0f, -90f);
                        newSword.transform.localScale = new Vector3(0.01598134f, 0.01902541f, 0.01598134f);
                        Debug.Log("Rune sword attached.");
                    } else if (itemInSlot.item.name.Contains("BasicSword")) {
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
                } else {
                    Debug.LogWarning("No torso found in the scene..... for some reason...");
                }
            }
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
        int id = Random.Range(0, itemsToPickup.Length);
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
                if (itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
                SendCurrentInventoryToState();
            }
            return item;
        } else {
            return null;
        }
    }
}