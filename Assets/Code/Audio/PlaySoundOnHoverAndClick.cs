using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnHoverAndClick : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource hover;
    public AudioSource confirm;
    public AudioSource denied;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.Play();
    }

    public void PlayConfirmed() {
        confirm.Play();
    }

    public void PlayDenied() {
        denied.Play();
    }
}
