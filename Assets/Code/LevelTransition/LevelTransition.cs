using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string targetScene;
    private string currentScene;
    public Vector3 nextPosition;
    public Vector3 returnPosition;
    GameObject levelChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(FadeToScene());
        }
    }

    private IEnumerator FadeToScene() {
            Debug.Log("Player made contact");
            currentScene = SceneManager.GetActiveScene().name;

            SceneTransitionManager.Instance.targetScene = targetScene;
            SceneTransitionManager.Instance.playerSpawnPosition = nextPosition;

            FadeTransition transition = levelChanger.GetComponent<FadeTransition>();
            transition.animator = levelChanger.GetComponent<Animator>();
            transition.sceneMusic = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioManager>().sceneMusic;
            yield return StartCoroutine(transition.PlayFadeOutFast());

            Debug.Log("Going to loading screen");
            SceneManager.LoadScene("LoadingScene");
    }
}