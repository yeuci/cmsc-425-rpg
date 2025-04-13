using UnityEngine;
using System.Collections;

public class OpenSkillcheck : MonoBehaviour
{
    public GameObject minigamePrefab; 
    // The Canvas where the minigame will show up
    public Canvas canvas;

    public bool isMinigameSuccessful;

    // Current instance of the minigame
    private GameObject currentMinigameInstance = null;
    private Skillcheck skillcheck = null;

    public IEnumerator StartMinigame() {
        // Instantiates the minigame if there is not one running
        if (minigamePrefab != null && currentMinigameInstance == null) {

            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            skillcheck = currentMinigameInstance.GetComponent<Skillcheck>();

            yield return StartCoroutine(skillcheck.CheckSkillcheck());

            isMinigameSuccessful = skillcheck.isMinigameSuccessful;
        }

    }

}
