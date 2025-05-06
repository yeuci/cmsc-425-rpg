using System;
using System.Collections;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject player;
    AudioSource spikeAudio;
    CharacterController controller;
    Boolean activated = false;
    void Start()
    {
        BoxCollider hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
        player = GameObject.FindGameObjectWithTag("Player");
        spikeAudio = GetComponent<AudioSource>();
        controller = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Entity playerE = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<Entity>();
            if(playerE != null) {
                playerE.remainingHP -= 15;
            }
            
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            StartCoroutine(Knockback(knockbackDirection));

            if(!activated){
                activated = true;
                StartCoroutine(SpikeTrapActivation());
            }
        }
    }

    //Trap movement should last half a second
    IEnumerator SpikeTrapActivation() {
        //The false floor is no longer a child of the spikes
        Transform spikes = transform.GetChild(0);
        int movementDirection = 1;
        spikeAudio.Play();
        for(int i = 0; i < 50; i++) {
            if(i == 25) {
                movementDirection = -1;
            }
            yield return new WaitForSeconds(.01f);
            spikes.Translate(new Vector3(0f,0f,movementDirection*1.547212f/25));
        }
        activated = false;
        yield return null;
    }

    IEnumerator Knockback(Vector3 direction)
    {
        float knockbackDuration = 0.2f;
        float timer = 0f;
        float knockbackSpeed = 25f;

        while (timer < knockbackDuration)
        {
            controller.Move(direction * knockbackSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
