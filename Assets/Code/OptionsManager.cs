using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;
    public GameObject optionsMenu;
    public static bool isOpen = false;

    void Awake()
    {
        Debug.Log("OptionsManager AWAKE.");

        #if !UNITY_EDITOR
            Screen.fullScreen = false;
        #endif

        optionsMenu.SetActive(false);
        isOpen = false;
        
        if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 1f);
            Load();
        } else {
            Load();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isOpen) {
            optionsMenu.SetActive(true);
            isOpen = true;
        }
    }

void Update() {
    if (Input.GetKeyDown(KeyCode.O)) {
        OpenOptions();
    }
}
    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        ChangeVolume();
    }

    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void CloseOptions() {
        optionsMenu.SetActive(false);
    }

    public void OpenOptions() {
        optionsMenu.SetActive(true);
    }

    public void OnFullScreenToggle() {
        int fullscreen = PlayerPrefs.GetInt("fullscreen", 0);
        if (fullscreen == 0) {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("fullscreen", 1);
        } else {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("fullscreen", 0);
        }
    }
}
