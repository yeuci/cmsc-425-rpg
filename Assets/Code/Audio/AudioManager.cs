using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource sceneMusic;
    public AudioSource hover;
    public AudioSource confirm;
    public AudioSource denied;
    public AudioSource use;
    public AudioSource victoryMusic;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hover != null) {
            hover.Play();
        }
        
    }

    public void PlayHover() {
        hover.Play();
    }

    public void PlayConfirmed() {
        confirm.Play();
    }

    public void PlayDenied() {
        denied.Play();
    }

    public void PlayUse() {
        use.Play();
    }

    public void playVictory() {
        victoryMusic.Play();
    }
}
