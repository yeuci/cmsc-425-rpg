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
            if (player != null)
            {
                player.transform.position = SceneTransitionManager.Instance.playerSpawnPosition;
            }

            SceneTransitionManager.Instance.shouldTeleportOnSceneLoad = false;
        }
    }
}
