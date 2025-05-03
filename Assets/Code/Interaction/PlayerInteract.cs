using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out NPCInteractable npc)) {
                    npc.Interact();
                } 
            }
        }
    }
}
