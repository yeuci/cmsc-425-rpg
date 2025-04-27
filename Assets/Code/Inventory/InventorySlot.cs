using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    public Image image;
    public Color selectedColor, notSelectedColor;
    [HideInInspector] public int uuid;

    public void Awake() {
        Deselect();
    }

    public void Start()
    {
        uuid = Random.Range(10000, 99999);
    }

    public void Select() {
        image.color = selectedColor;
    }

    public void Deselect() {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            Debug.Log(inventoryItem.item.name);
        }
    }
}
