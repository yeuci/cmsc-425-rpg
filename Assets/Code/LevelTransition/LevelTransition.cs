using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string targetScene;
    private string currentScene;
    public Vector3 nextPosition;
    public Vector3 returnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player made contact");
            currentScene = SceneManager.GetActiveScene().name;

            SceneTransitionManager.Instance.targetScene = targetScene;
            SceneTransitionManager.Instance.playerSpawnPosition = nextPosition;

            Debug.Log("Going to loading screen");
            SceneManager.LoadScene("LoadingScene");
        }
    }
}