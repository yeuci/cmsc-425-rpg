using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework.Internal;
using System.Text.RegularExpressions;
using System.Collections;
using Unity.VisualScripting;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    public GameObject countTextContainer;
    public Item item;
    public GameObject inventoryPopupPanel;
    // public 
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public InventoryManager iMEntity;
    [HideInInspector] public Entity playerEntity;
    [HideInInspector] public InventorySlot originalSlot;
    [HideInInspector] public GameObject currentPopupPanel;

     public int uuid;

    public void InitializeItem(Item newItem) {
        RectTransform rectTransform = GetComponent<RectTransform>();

        item = newItem;
        image.sprite = newItem.image;
        rectTransform.sizeDelta = new Vector2(84, 84);
        RefreshCount();
    }

    void Start() {
        iMEntity = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
        playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();

        uuid = Random.Range(10000, 99999);

        if (iMEntity == null) {
            Debug.LogWarning("No InventoryManager found in the scene. -- START");
        }
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        bool textVisible = count > 1;
        countText.gameObject.SetActive(textVisible);
        countTextContainer.SetActive(textVisible);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;

        // save original slot
        originalSlot = parentAfterDrag.GetComponent<InventorySlot>();

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

        if (eventData != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null) {
            iMEntity.ChangeSelectedSlot(iMEntity.selectedSlot);
            iMEntity.SendCurrentInventoryToState();
        } else {
            Debug.Log("Item with ID " + uuid + " attempted to be dropped outside of inventory slots.");
            // dont destroy item for now....

            // gameObject.SetActive(false); 
            // Destroy(gameObject);
            // iMEntity.UpdateInventoryUIWithItemSave();
            // iMEntity.ChangeSelectedSlot(iMEntity.selectedSlot);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HOVERING AN ITEM");
        displayItemInformation(item.name, item.getItemDescription(), image.transform);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (currentPopupPanel != null) {
            Destroy(currentPopupPanel);
        }
    }

    void displayItemInformation(string itemName, string itemDescription, Transform itemPos) {
        if (currentPopupPanel == null) {
            Destroy(currentPopupPanel);
        }

        currentPopupPanel = Instantiate(inventoryPopupPanel, itemPos);
        currentPopupPanel.transform.localPosition = new Vector3(0, 150, 0);
        currentPopupPanel.GetComponent<PopupInfo>().Setup(itemName, itemDescription);
    }
}