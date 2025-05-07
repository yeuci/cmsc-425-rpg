using UnityEngine;

public class GateInteractable : MonoBehaviour, Interactable
{
    public LeverLiftGate leverLiftGate;

    public void Interact(Transform transform)
    {
        Debug.Log("Interacting with door.");

        if (leverLiftGate != null)
        {
            leverLiftGate.ActivateLeverAndGateInteractable();
            Debug.Log("LeverLiftGate activated.");
        }
        else
        {
            Debug.LogError("LeverLiftGate not assigned in the inspector.");
        }
    }

    public string GetInteractText()
    {
        return "Open Gate";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
