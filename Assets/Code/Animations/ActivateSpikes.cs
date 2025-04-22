using System.Collections;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject spikes;
    GameObject player;
    AudioSource spikeAudio;
    void Start()
    {
        BoxCollider hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
        spikes = transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        spikeAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            StartCoroutine(SpikeTrapActivation());
        }
    }

    //Trap movement should last half a second
    IEnumerator SpikeTrapActivation() {
        //The false floor is no longer a child of the spikes
        transform.parent = null;
        int movementDirection = 1;
        spikeAudio.Play();
        for(int i = 0; i <= 50; i++) {
            if(i == 25) {
                movementDirection = -1;
            }
            yield return new WaitForSeconds(.01f);
            spikes.transform.Translate(new Vector3(0f,0f,movementDirection*1.547212f/25));
        }
        transform.parent = spikes.transform;
        yield return null;
    }
}
