using UnityEngine;

public interface Interactable
{
    void Interact(Transform transform);
    string GetInteractText();
    Transform GetTransform();
}
