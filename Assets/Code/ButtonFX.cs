using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource hoverAudioSource;
    public AudioSource clickAudioSource;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverAudioSource != null && !hoverAudioSource.isPlaying)
        {
            hoverAudioSource.Play();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickAudioSource != null)
        {
            clickAudioSource.Play();
        }
    }
}
