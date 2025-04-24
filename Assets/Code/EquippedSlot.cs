using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IDropHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image image;
    public Color selectedColor, notSelectedColor;

    GameObject playerObject;
    Entity playerEntity;
    [HideInInspector] public int uuid;

    public void Start()
    {
        uuid = Random.Range(10000, 99999);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerEntity = playerObject.GetComponent<Entity>();

    }
    public void Select() {
        image.color = selectedColor;
    }
    public void Deselect() {
        image.color = notSelectedColor;
    }
    public void Awake() {
        Deselect();
    }

    //This is the primary change from Inventory Slot
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            Debug.Log(inventoryItem.item.name +" in equipped gear");
            playerEntity.equippedGear[0] = inventoryItem.item;
        }
    }
}
