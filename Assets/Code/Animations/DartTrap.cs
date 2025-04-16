using UnityEngine;

public class DartTrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dartModel;
    GameObject dartPanel; //This is where the darts will originate from
    void Start()
    {   
        BoxCollider pressurePlate = GetComponent<BoxCollider>();
        pressurePlate.isTrigger = true;
        dartPanel = transform.parent.gameObject;
        Debug.Log("Dart Panel: "+dartPanel);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            Debug.Log("Player Triggered dart trap");
            Instantiate(dartModel, dartPanel.transform);
        }
    }
}
