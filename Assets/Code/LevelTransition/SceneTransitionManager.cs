using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public string targetScene;
    public Vector3 playerSpawnPosition;
    public bool shouldTeleportOnSceneLoad = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // If condition for checking if new game with no level transition yet 
        if (playerSpawnPosition == Vector3.zero) {
            playerSpawnPosition = new Vector3(14f, 2.5f, 15f);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}