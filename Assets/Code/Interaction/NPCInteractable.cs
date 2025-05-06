using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactable
{
    [SerializeField] private string interactText;
    [SerializeField] private int interactionAmount = 0;
    [SerializeField] public int npcType = 0; // 0 = Starting NPC, 1 = Merchant

    public void Interact(Transform transform) {
        Debug.Log("Interact");
        if (npcType == 0) {
            DialogueManager diaContainer = GameObject.FindGameObjectWithTag("dialogue_container")?.GetComponent<DialogueManager>();
            if (diaContainer != null) {
                diaContainer.ShowDialogueBox();
            } else {
                Debug.LogError("DialogueManager not found in the scene.");
            }
        }
        interactionAmount++;
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }
}
