using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    public void Settings() {
        Debug.Log("Settings button clicked.");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
