using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactable
{
    [SerializeField] private string interactText;
    [SerializeField] private int interactionAmount = 0;

    public void Interact(Transform transform) {
        Debug.Log("Interact");
        interactionAmount++;
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }
}
