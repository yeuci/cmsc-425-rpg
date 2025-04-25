using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections;

public class SaveGameLoader : MonoBehaviour
{
    private string folderName = "CMSC425_SaveGames";
    private string savePath;
    private ItemSave[] loadedInventory = new ItemSave[25];
    
    [HideInInspector] Entity playerEntity;
    [HideInInspector] PlayerManager playerManager;
    [HideInInspector] InventoryManager iMEntity;

    private void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();

        DontDestroyOnLoad(this.gameObject);
        savePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), folderName);
    }

    private Item GetItem(string name) {
        foreach (var item in playerManager.itemsArray) {
            if (item && item.name == name) {
                return item;
            }
        }
        return null;
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

        playerManager.playerCanCollide = false;

        string[] lines = File.ReadAllLines(lastSaveFile);
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        
        // Stats variables
        int level = 1;
        float experience = 0;
        float health = 100;
        float attack = 10;
        float defense = 10;
        float speed = 10;
        float magic = 5;
        float expToNext = 100;

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
            else if (line.StartsWith("Level:"))
            {
                level = int.Parse(line.Replace("Level:", "").Trim());
            }
            else if (line.StartsWith("Experience:"))
            {
                experience = float.Parse(line.Replace("Experience:", "").Trim());
            }
            else if (line.StartsWith("Health:"))
            {
                health = float.Parse(line.Replace("Health:", "").Trim());
            }
            else if (line.StartsWith("Attack:"))
            {
                attack = float.Parse(line.Replace("Attack:", "").Trim());
            }
            else if (line.StartsWith("Defense:"))
            {
                defense = float.Parse(line.Replace("Defense:", "").Trim());
            }
            else if (line.StartsWith("Speed:"))
            {
                speed = float.Parse(line.Replace("Speed:", "").Trim());
            }
            else if (line.StartsWith("Magic:"))
            {
                magic = float.Parse(line.Replace("Magic:", "").Trim());
            }
            else if (line.StartsWith("ExpToNext:"))
            {
                expToNext = float.Parse(line.Replace("ExpToNext:", "").Trim());
            } else if (line.StartsWith("Inventory:")) {
                string raw = line.Replace("Inventory:", "").Trim();

                raw = raw.TrimStart('[').TrimEnd(']');

                string[] entries = raw.Split(new string[] { "},", "null," }, System.StringSplitOptions.None);

                for (int i = 0; i < entries.Length; i++)
                {
                    string entry = entries[i].Trim();

                    if (entry == "null" || string.IsNullOrWhiteSpace(entry))
                    {
                        loadedInventory[i] = null;
                    }
                    else
                    {
                        entry = entry.TrimStart('{').TrimEnd('}', ',').Trim();
                        string[] parts = entry.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                        int count = 0;
                        string itemName = "";

                        foreach (var part in parts)
                        {
                            string[] kv = part.Split('=');
                            if (kv.Length == 2)
                            {
                                string key = kv[0].Trim();
                                string value = kv[1].Trim().Trim('"');

                                if (key == "count")
                                    int.TryParse(value, out count);
                                else if (key == "item")
                                    itemName = value;
                            }
                        }

                        ItemSave itemSave = new ItemSave();
                        itemSave.count = count;
                        itemSave.item = itemName;
                        itemSave.itemData = GetItem(itemName);

                        loadedInventory[i] = itemSave;
                    }
                }
            }
        }

        StartCoroutine(LoadSceneAndApplyState(position, rotation, level, experience, health, attack, defense, speed, magic, expToNext));
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

    private IEnumerator LoadSceneAndApplyState(Vector3 position, Vector3 rotation, 
                                            int level, float experience, float health, 
                                            float attack, float defense, float speed, 
                                            float magic, float expToNext)
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
        
        Debug.Log("Attempting to find player...");
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            Debug.Log("Found: " + player.name);
            
            MonoBehaviour[] playerScripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in playerScripts)
            {
                if (script != null && (script.GetType().Name.Contains("Movement") || 
                                    script.GetType().Name.Contains("Controller") || 
                                    script.GetType().Name.Contains("Input") ||
                                    script.GetType().Name.Contains("Move"))
                                    ) 
                {
                    script.enabled = false;
                    Debug.Log($"Temporarily disabled: {script.GetType().Name}");
                }
            }
            
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                Debug.Log("Temporarily disabled Character Controller");
            }
            
            player.transform.position = position;
            player.transform.eulerAngles = rotation;

            Debug.Log($"Player position set to: {position}");
            Debug.Log($"Player rotation set to: {rotation}");

            playerEntity = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
            
            if (playerEntity != null && playerEntity.stats != null)
            {
                Debug.Log("Applying stats to player...");
                playerEntity.stats.level = level;
                playerEntity.stats.experience = experience;
                playerEntity.stats.health = health;
                playerEntity.stats.attack = attack;
                playerEntity.stats.defense = defense;
                playerEntity.stats.speed = speed;
                playerEntity.stats.magic = magic;
                playerEntity.stats.expToNext = expToNext;

                for (int i = 0; i < 25; i++) {
                    if (loadedInventory[i] == null)
                    {
                        playerEntity.inventory[i] = null;
                        continue;
                    }
                    playerEntity.inventory[i] = new ItemSave();
                    playerEntity.inventory[i].count = loadedInventory[i].count;
                    playerEntity.inventory[i].item = loadedInventory[i].item;
                    playerEntity.inventory[i].itemData = GetItem(loadedInventory[i].item);

                    Debug.Log($"Item {i}: {playerEntity.inventory[i].item} with count {playerEntity.inventory[i].count} i = {i}");
                }

                // refresh the ui
                iMEntity = GameObject.FindGameObjectWithTag("InventoryManager")?.GetComponent<InventoryManager>();
                iMEntity.UpdateInventoryUIWithItemSave();

                // destroy all defeated enemies
                GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag("Enemy"))
                    {
                        Entity entity = obj.GetComponent<Entity>();
                        if (entity != null)
                        {
                            if (playerManager.defeatedEnemies.Contains(entity.enemyId)) {
                                obj.gameObject.SetActive(false);
                                Destroy(obj);
                            }
                        }
                    }
                }

                playerManager.playerCanCollide = true;

                Debug.Log("Player stats restored from save");
            }
            else
            {
                Debug.LogWarning("Player Entity component or stats not found! ");
            }
            
            yield return null;
            
            if (controller != null)
            {
                controller.enabled = true;
                Debug.Log("Re-enabled Character Controller");
            }
            
            foreach (MonoBehaviour script in playerScripts)
            {
                if (script != null && !script.enabled)
                {
                    script.enabled = true;
                    Debug.Log($"Re-enabled: {script.GetType().Name}");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player not found in Dungeon scene!");
        }
        
        Debug.Log("LoadSceneAndApplyState completed");
    }
}