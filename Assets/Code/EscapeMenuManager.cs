using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour
{

    public GameObject escapeMenu;
    public AudioSource pause;
    public AudioSource unpause;
    AudioSource dungeonMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        escapeMenu.SetActive(false);
        dungeonMusic = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.SetActive(!escapeMenu.activeSelf);

            if (escapeMenu.activeSelf)
            {
                pause.Play();
                StartCoroutine(FadeAudio.FadeOut(dungeonMusic, 0.3f));

            }
            else
            {
                unpause.Play();
                StartCoroutine(FadeAudio.FadeIn(dungeonMusic, 0.3f, PlayerPrefs.GetFloat("musicVolume")));
            }

        }
    }

    public void OnResume() {
        unpause.Play();
        StartCoroutine(FadeAudio.FadeIn(dungeonMusic, 0.5f, PlayerPrefs.GetFloat("musicVolume")));
        escapeMenu.SetActive(!escapeMenu.activeSelf);
        Debug.Log("Escape menu toggled! " + escapeMenu.activeSelf);
    }

    public void OnSave() {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            SaveGame saveGame = player.GetComponent<SaveGame>();
            if (saveGame != null)
            {
                saveGame.Save();
            }
            else
            {
                Debug.LogWarning("SaveGame component not found on Player!");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }

    }

    public void OnQuit() {
        SceneManager.LoadScene("MainMenu");
    }
}
