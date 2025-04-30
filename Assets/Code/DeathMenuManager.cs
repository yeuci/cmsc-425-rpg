 using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{

    public GameObject deathMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        deathMenu.SetActive(false);
    }

    public void Setup() {
        deathMenu.SetActive(true);
    }

    public void OnLastSave() {
        SceneManager.LoadScene("DungeonMap");
        // TODO: check if there is a last save, if not just load the dungeonmap
    }

    public void OnNewGame() {
        SceneManager.LoadScene("DungeonMap");
        // TODO: reset playermanager
    }

    public void OnQuit() {
        SceneManager.LoadScene("MainMenu");
    }
}
