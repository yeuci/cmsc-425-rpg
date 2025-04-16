using UnityEngine;

public class DartTrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dartModel;
    void Start()
    {
        Collider hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            
            Debug.Log("Made contact with trap");
            
        }
    }
}
