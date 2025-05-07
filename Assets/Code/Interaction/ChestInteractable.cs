using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChestInteractable : MonoBehaviour, Interactable
{

    public List<Item> items = new List<Item>();
    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
    }

    public void Interact(Transform transform)
    {
        List<Item> itemsAdded = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                continue;
            }

            Item item = items[i];
            inventoryManager.AddItem(item);
            items[i] = null;

            Debug.Log("Item added to inventory: " + item.name);
            itemsAdded.Add(item);
        }

        inventoryManager.SendCurrentInventoryToState();

        // TODO: DISPLAY ITEMS ADDED TO INVENTORY
        StartCoroutine(DisplayItemModalsWithDelay(itemsAdded));
    }

    private IEnumerator DisplayItemModalsWithDelay(List<Item> itemsAdded)
    {
    for (int i = 0; i < itemsAdded.Count; i++)
    {
        ItemModalManager.Instance.ShowItemModal(itemsAdded[i]);

        yield return new WaitForSeconds(0.25f);
    }
    }

    public string GetInteractText()
    {
        return "Open Chest";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}   
