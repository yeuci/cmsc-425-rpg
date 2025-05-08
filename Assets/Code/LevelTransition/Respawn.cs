using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.shouldTeleportOnSceneLoad)
        {
            Debug.Log("Spawning player at: " + SceneTransitionManager.Instance.playerSpawnPosition);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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
            if (player != null)
            {
                player.transform.position = SceneTransitionManager.Instance.playerSpawnPosition;
            }

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

            SceneTransitionManager.Instance.shouldTeleportOnSceneLoad = false;
        }
    }
}
