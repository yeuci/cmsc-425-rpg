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
    }

    public void NewGame() {
        if (!string.IsNullOrEmpty(gameScene))
        {
            Invoke(nameof(LoadGameScene), 0.2f);
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

    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
}
