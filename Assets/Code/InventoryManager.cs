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
    public bool equipped = false;

    private void Start() {
        inventoryGroup.SetActive(false);
        ChangeSelectedSlot(0);
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

                AddItemToPlayerInventory(spawnedItem);
                return true;
            }
        }
        return false;
    }

    void AddItemToPlayerInventory(InventoryItem inItem) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player) {
            Entity playerEntity = player.GetComponent<Entity>();
            if (playerEntity) {
                Debug.Log("----- IMPORTANT -------> " + inItem.item.name);
                playerEntity.inventory.Add(inItem);
                playerEntity.equippedGear[playerEntity.equippedGearCount] = inItem.item;
                playerEntity.equippedGearCount++;
                Debug.Log("Player entity found. Adding item to inventory.");
            } else {
                Debug.LogWarning("No player entity found. Can't add item to inventory.");
            }
        } else {
            Debug.LogWarning("No player found in the scene. Can't add item to inventory.");
        }
    }

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
            }
            return item;
        } else {
            return null;
        }
    }
}