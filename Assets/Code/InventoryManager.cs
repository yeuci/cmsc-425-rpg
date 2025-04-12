using NUnit.Framework.Internal;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject inventoryGroup;

    public int maxStackedItems = 4;

    int selectedSlot = -1;

    private void Start() {
        inventoryGroup.SetActive(false);
        ChangeSelectedSlot(0);
    }
    
    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }


    // TEST SUITE FOR ADDING ITEMS TO INVENTORY LOGIC
    public Item[] itemsToPickup;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryGroup.SetActive(!inventoryGroup.activeSelf);
        }

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
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
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
