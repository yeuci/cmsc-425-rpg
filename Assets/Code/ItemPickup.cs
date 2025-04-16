using UnityEngine;

public class ItemPickup : MonoBehaviour
{   
    public Item itemToPickup;

    [HideInInspector] public SphereCollider pickup;
    [HideInInspector] public InventoryManager iMEntity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iMEntity = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();

        pickup = gameObject.AddComponent<SphereCollider>();
        pickup.radius = 1f;
        pickup.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            //Add logic to add the item to the inventory here.
            if (iMEntity) {
                iMEntity.AddItem(itemToPickup);
                Debug.Log("Item picked up: " + itemToPickup.name);
                Destroy(gameObject);
            } else {
                Debug.LogWarning("No InventoryManager found in the scene. -- ONTRIGGERENTER");
            }
        }
    }
}
