using UnityEngine;
using System.IO;

public class SaveGame : MonoBehaviour
{
    public Transform player;
    public string folderName = "CMSC425_SaveGames";

    private string savePath;

    void Start()
    {
        savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), folderName);

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Save();
        }
    }

    void Save()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned!");
            return;
        }

        int saveID = Random.Range(10000, 99999);

        Vector3 pos = player.position;
        Vector3 rot = player.eulerAngles;

        string data = $"Save ID: {saveID}\nPosition: {pos.x}, {pos.y}, {pos.z}\nRotation: {rot.x}, {rot.y}, {rot.z}";

        string fileName = $"savegame_{saveID}.txt";
        string fullPath = Path.Combine(savePath, fileName);

        File.WriteAllText(fullPath, data);

        Debug.Log($"Game saved to: {fullPath}");
    }
}
