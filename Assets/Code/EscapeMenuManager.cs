using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour
{

    public GameObject escapeMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        escapeMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.SetActive(!escapeMenu.activeSelf);
            Debug.Log("Escape menu toggled! " + escapeMenu.activeSelf);
        }
    }

    public void OnResume() {
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
