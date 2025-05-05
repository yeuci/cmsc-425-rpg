using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string targetScene;
    private string currentScene;
    public Vector3 nextPosition;
    public Vector3 returnPosition;

    private bool canTrigger = false;

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
            currentScene = SceneManager.GetActiveScene().name;
            returnPosition = nextPosition;
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
