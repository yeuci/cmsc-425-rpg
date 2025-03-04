using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource hoverAudioSource;
    public AudioSource clickAudioSource;
    [SerializeField] private string gameScene;

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

        if (!string.IsNullOrEmpty(gameScene))
        {
            Invoke(nameof(LoadGameScene), 0.2f);
        }
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
}
