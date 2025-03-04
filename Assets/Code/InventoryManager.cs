using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryGroup;

    private void Start()
    {
        inventoryGroup.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryGroup.SetActive(!inventoryGroup.activeSelf);
        }
    }
}
