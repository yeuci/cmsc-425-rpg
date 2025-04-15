using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject spikes;
    void Start()
    {
        BoxCollider hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
        spikes = GameObject.FindGameObjectWithTag("TrapDynamicComponent");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            spikes.transform.Translate(new Vector3(0f,0f,1.547212f));
        }
    }
}
