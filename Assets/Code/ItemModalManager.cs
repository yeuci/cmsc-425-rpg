using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ItemModalManager : MonoBehaviour
{
    public static ItemModalManager Instance;

    public GameObject itemModalPrefab;
    public Transform modalContainer;

    // public Item[] items;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        if (modalContainer == null)
        {
            Debug.LogError("Modal container not assigned");
            return;
        }
    }

    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     Debug.Log("M key pressed, showing item modal.");
        //     Instance.ShowItemModal(items[Random.Range(0, items.Length)]);
        // }   
    }

    public void ShowItemModal(Item item)
    {
        GameObject modal = Instantiate(itemModalPrefab, modalContainer);
        modal.transform.SetAsLastSibling();

        TextMeshProUGUI nameText = modal.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = item.name;

        Transform imageTransform = modal.transform.Find("ItemImage");
        if (imageTransform != null)
        {
            Image itemImage = imageTransform.GetComponent<Image>();
            if (itemImage != null && item.image != null)
            {
                itemImage.sprite = item.image;
            }
        }

        StartCoroutine(FadeAndDestroy(modal, 5f));
    }

    private IEnumerator FadeAndDestroy(GameObject modal, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (modal == null)
            yield break;

        CanvasGroup cg = modal.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = modal.AddComponent<CanvasGroup>();

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            if (cg == null)
                yield break;

            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            t += Time.deltaTime;

            yield return null;
        }

        if (modal != null)
            Destroy(modal);
    }

}
