using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    [Header("UI")]
    public Image image;

    public Item item;
    [HideInInspector] public Transform parentAfterDrag;


    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
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
    }

    void Start() {
        gameObject.AddComponent<SphereCollider>();
            SphereCollider pickupBox = gameObject.GetComponent<SphereCollider>();
            pickupBox.radius = 3f;
            pickupBox.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Entity player = playerObject.GetComponent<Entity>();
            player.equippedGear[0] = item;
            Destroy(gameObject);
        }
    }
}
