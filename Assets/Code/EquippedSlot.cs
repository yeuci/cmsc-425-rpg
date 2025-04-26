using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IDropHandler
{


    public Image image;
    public ItemType itemType;
    public Color selectedColor, notSelectedColor;
    [HideInInspector] public int uuid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uuid = Random.Range(10000, 99999);
    }

    public void Awake() {
        Deselect();
    }

    public void Select() {
        image.color = selectedColor;
    }

    public void Deselect() {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount == 0 && eventData.pointerDrag.GetComponent<InventoryItem>().item.type == itemType) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            Debug.Log(inventoryItem.item.name);
        }
    }
}
