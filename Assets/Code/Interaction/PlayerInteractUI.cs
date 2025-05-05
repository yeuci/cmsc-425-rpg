using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    
    [SerializeField] private GameObject interactUIContainer;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            ShowInteractUI(playerInteract.GetInteractableObject());
        }
        else
        {
            HideInteractUI();
        }
    }

    private void ShowInteractUI(Interactable interactable)
    {
        interactUIContainer.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }
    
    private void HideInteractUI()
    {
        interactUIContainer.SetActive(false);
    }
}
