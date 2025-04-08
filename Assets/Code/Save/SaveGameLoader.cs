using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections;

public class SaveGameLoader : MonoBehaviour
{
    public string folderName = "CMSC425_SaveGames";
    private string savePath;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), folderName);
    }

    public void LoadLastSave()
    {
        // REWORK NEEDED!!!
        Debug.Log("Loading last save...");
        if (!Directory.Exists(savePath))
        {
            // will add a error message in UI later


            Debug.LogWarning("No save folder found. --> " + savePath);
            return;
        }

        string lastSaveFile = Directory.GetFiles(savePath, "savegame_*.txt")
            .OrderByDescending(File.GetLastWriteTime)
            .FirstOrDefault();

        Debug.Log("Last save file: " + lastSaveFile);

        if (string.IsNullOrEmpty(lastSaveFile))
        {
            // will add a error message in UI later

            Debug.LogWarning("No save file found.");
            return;
        }

        string[] lines = File.ReadAllLines(lastSaveFile);
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        // need to compress later, test for now
        foreach (string line in lines)
        {
            if (line.StartsWith("Position:"))
            {
                position = ParseVector3(line.Replace("Position:", "").Trim());
            }
            else if (line.StartsWith("Rotation:"))
            {
                rotation = ParseVector3(line.Replace("Rotation:", "").Trim());
            }
        }

        StartCoroutine(LoadSceneAndApplyState(position, rotation));
    }

    private Vector3 ParseVector3(string line)
    {
        string[] values = line.Split(',');
        return new Vector3(
            float.Parse(values[0]),
            float.Parse(values[1]),
            float.Parse(values[2])
        );
    }

    private IEnumerator LoadSceneAndApplyState(Vector3 position, Vector3 rotation)
    {
        Debug.Log("LoadSceneAndApplyState started");
        Debug.Log("Loading DungeonMap scene...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("DungeonMap");
        
        Debug.Log("Waiting for scene to load completely...");
        while (!asyncLoad.isDone)
        {
            Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");
            yield return null;
        }
        
        Debug.Log("DungeonMap scene loaded.");
        
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Attempting to find player...");
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            Debug.Log("Found: " + player.name);
            
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                Debug.Log("Temporarily disabled Character Controller");
            }
            
            player.transform.position = position;
            player.transform.eulerAngles = rotation;
            
            if (controller != null)
            {
                StartCoroutine(ReenableController(controller));
            }

            Debug.Log($"Player position set to: {position}");
            Debug.Log($"Player rotation set to: {rotation}");
        }
        else
        {
            Debug.LogWarning("Player not found in Dungeon scene!");
        }
        
        Debug.Log("LoadSceneAndApplyState completed");
    }

    private IEnumerator ReenableController(CharacterController controller)
    {
        yield return new WaitForSeconds(0.1f);
        controller.enabled = true;
        Debug.Log("Re-enabled Character Controller");
    }
}
