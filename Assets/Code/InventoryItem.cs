using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework.Internal;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    public Item item;
    // public 
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public InventoryManager iMEntity;
    [HideInInspector] public int uuid;

    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    void Start() {
        iMEntity = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
        uuid = Random.Range(10000, 99999);

        if (iMEntity == null) {
            Debug.LogWarning("No InventoryManager found in the scene. -- START");
        }
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        bool textVisible = count > 1;
        countText.gameObject.SetActive(textVisible);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); 
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        image.raycastTarget = true; 
        transform.SetParent(parentAfterDrag, false); 
        transform.position = parentAfterDrag.position;

        Debug.Log("Item with ID " + uuid + " dropped on " + eventData.pointerCurrentRaycast.gameObject.name + " with ID " + eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>().uuid);
        iMEntity.ChangeSelectedSlot(iMEntity.selectedSlot);
    }
}
