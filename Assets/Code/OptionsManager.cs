using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;
    public GameObject optionsMenu;

    void Awake()
    {
        Debug.Log("OptionsManager started.");

        #if !UNITY_EDITOR
            Screen.fullScreen = false;
        #endif

        optionsMenu.SetActive(false);
        
        if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1f);
            Load();
        } else {
            Load();
        }
        Debug.Log("Volume is... : " + volumeSlider.value);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionsMenu.SetActive(true);
    }

void Update() {
    if (Input.GetKeyDown(KeyCode.O)) {
        OpenOptions();
    }
}
    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        Save();
        Debug.Log("Volume changed to: " + volumeSlider.value);
    }

    private void Load() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void CloseOptions() {
        optionsMenu.SetActive(false);
        Debug.Log("Options closed.");
    }

    public void OpenOptions() {
        optionsMenu.SetActive(true);
        Debug.Log("Options opened.");
    }

    public void OnFullScreenToggle() {
        int fullscreen = PlayerPrefs.GetInt("fullscreen", 0);
        if (fullscreen == 0) {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("fullscreen", 1);
            Debug.Log("Fullscreen enabled.");
        } else {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("fullscreen", 0);
            Debug.Log("Fullscreen disabled.");
        }
    }
}
