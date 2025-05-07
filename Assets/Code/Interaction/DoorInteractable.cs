using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    public DoorController doorController;

    public void Interact(Transform transform)
    {
        Debug.Log("Interacting with door.");

        if (doorController != null)
        {
            doorController.OpenDoorInteractable();
        }
        else
        {
            Debug.LogError("DoorController not assigned in the inspector.");
        }
    }

    public string GetInteractText()
    {
        return "Open Door";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
