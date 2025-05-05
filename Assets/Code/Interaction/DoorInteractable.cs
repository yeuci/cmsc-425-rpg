using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{

    public void Interact(Transform transform)
    {
        Debug.Log("Interacting with door.");
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
