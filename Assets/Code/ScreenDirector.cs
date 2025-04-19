using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenDirector : MonoBehaviour
{
    public GameObject deathMenu;
    void Start()
    {
       deathMenu.SetActive(true);
    } 
    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNewGame() {
        SceneManager.LoadScene("DungeonMap");
    }

    public void LoadRespawn() {
        Debug.Log("Respawning. Using data from last save");
    }
}
