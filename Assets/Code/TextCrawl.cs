using System.Collections;
using UnityEngine;

public class TextCrawl : MonoBehaviour
{

    [SerializeField] private float speed = 20f;

    void Start()
    {
        StartCoroutine(LoadSelectedScene());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Camera.main.transform.up * speed * Time.deltaTime);
    }

    private IEnumerator LoadSelectedScene()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 55f)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Change KeyCode.Space to your desired key
            {
                break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PlayerManager playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();

        if (playerManager)
        {
            playerManager.isNewPlayer = false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("DungeonMap");
    }
}
