using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathMenuManager : MonoBehaviour
{

    public GameObject deathMenu;
    Animator animator;
    AudioManager musicManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        animator = GetComponent<Animator>();
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioManager>();
        deathMenu.SetActive(false);
    }

    public IEnumerator Setup() {
        deathMenu.SetActive(true);

        if (musicManager.sceneMusic != null) {
            musicManager.sceneMusic.Stop();
        }

        musicManager.playDefeat();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    public void OnLastSave() {
        SaveGameLoader loader = GameObject.FindGameObjectWithTag("SaveSystem").GetComponent<SaveGameLoader>();
        deathMenu.SetActive(false);
        loader.LoadLastSave();
    }

    public void OnNewGame() {
        SaveGameLoader loader = GameObject.FindGameObjectWithTag("SaveSystem").GetComponent<SaveGameLoader>();
        loader.NewGame();
    }

    public void OnQuit() {
        SceneManager.LoadScene("MainMenu");
    }
}
