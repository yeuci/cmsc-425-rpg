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
        yield return new WaitForSeconds(55f);
        
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();

        if (playerManager) {
            playerManager.isNewPlayer = false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("DungeonMap");
    }
}
