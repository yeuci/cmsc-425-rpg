using UnityEngine;
using System.IO;

public class SaveGame : MonoBehaviour
{
    public Transform player;
    public string folderName = "CMSC425_SaveGames";

    private string savePath;
    [HideInInspector] Entity playerEntity;
    [HideInInspector] PlayerManager playerManager;
    [HideInInspector] InventoryManager iMEntity;


    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
        iMEntity = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();


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
    public void Save()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform not assigned!");
            return;
        }

        if (playerEntity == null)
        {
            Debug.LogWarning("Player Entity component not found!");
            return;
        }

        iMEntity.SendCurrentInventoryToState();

        Stat playerStats = playerEntity.stats;
        if (playerStats == null)
        {
            Debug.LogWarning("Player Stats not found in Entity component!");
            return;
        }

        int saveID = Random.Range(10000, 99999);
        Vector3 pos = player.position;
        Vector3 rot = player.eulerAngles;

        string data = $"Save ID: {saveID}\n" +
                    $"Position: {pos.x}, {pos.y}, {pos.z}\n" +
                    $"Rotation: {rot.x}, {rot.y}, {rot.z}\n" +
                    $"Level: {playerStats.level}\n" +
                    $"Experience: {playerStats.experience}\n" +
                    $"Health: {playerStats.health}\n" +
                    $"Attack: {playerStats.attack}\n" +
                    $"Defense: {playerStats.defense}\n" +
                    $"Speed: {playerStats.speed}\n" +
                    $"Magic: {playerStats.magic}\n" +
                    $"ExpToNext: {playerStats.expToNext}";

        // Inventory
        string inventoryData = "Inventory: [ ";
        foreach (var itemSave in playerEntity.inventory)
        {
            if (itemSave != null)
            {
                inventoryData += $"{{count = {itemSave.count}, item = \"{itemSave.item}\"}}, ";
            }
            else
            {
                inventoryData += "null, ";
            }
        }
        inventoryData = inventoryData.TrimEnd(',', ' ') + " ]";
        data += "\n" + inventoryData;

        // Defeated enemies
        string enemiesData = "DefeatedEnemies: [ ";
        foreach (int id in playerManager.defeatedEnemies)
        {
            enemiesData += $"{id}, ";
        }
        enemiesData = enemiesData.TrimEnd(',', ' ') + " ]";
        data += "\n" + enemiesData;

        // Save to file
        string fileName = $"savegame_{saveID}.txt";
        string fullPath = Path.Combine(savePath, fileName);
        File.WriteAllText(fullPath, data);
        Debug.Log($"Game saved to: {fullPath}");
    }

}
