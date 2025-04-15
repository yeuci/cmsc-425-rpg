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

    public InventoryManager inventoryManager;

    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    void Start() {

        SphereCollider pickup = gameObject.AddComponent<SphereCollider>();
        pickup.radius = 1f;
        pickup.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            //Add logic to add the item to the inventory here.
            inventoryManager.AddItem(item);
            Debug.Log("Item picked up: " + item.name);
            Destroy(gameObject);
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

        GameObject iM = GameObject.FindGameObjectWithTag("InventoryManager");
        InventoryManager iMEntity = iM.GetComponent<InventoryManager>();
        
        iMEntity.ChangeSelectedSlot(iMEntity.selectedSlot);
    }
}
