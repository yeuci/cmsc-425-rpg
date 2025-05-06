using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public Image loadingImage;
    public string sceneToLoad;
    public Sprite[] loadingSprites; 
    private void Start()
    {
        StartCoroutine(UpdateTextCoroutine());
        StartCoroutine(LoadSelectedScene());
    }

    private IEnumerator UpdateTextCoroutine()
    {
        string[] loadingMessages = { 
            "When your health is low, health potions can make the difference between victory and defeat. Use them strategically to stay alive!",
            "Mages are weak against physical attacks, but strong against magic. Use this to your advantage.",
            "Retreating from a fight can be a good strategy. You can always come back later when you're stronger.",
            "The world is full of hidden treasures. Keep an eye out for secret paths and hidden chests.",
            "Some enemies are immune to certain types of damage. Experiment with different weapons and spells to find their weaknesses!",
            };

        int messageIndex = 0;
        int imageIndex = 0;
        while (true)
        {
            loadingText.text = loadingMessages[messageIndex]; 
            messageIndex = (messageIndex + 1) % loadingMessages.Length; 

            loadingImage.sprite = loadingSprites[imageIndex];
            imageIndex = (imageIndex + 1) % loadingSprites.Length;

            yield return new WaitForSeconds(4f); 
        }
    }

    private IEnumerator LoadSelectedScene()
    {
        yield return new WaitForSeconds(10f);

        string sceneToLoad = SceneTransitionManager.Instance.targetScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
