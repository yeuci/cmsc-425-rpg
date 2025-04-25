using UnityEngine;
using System.Collections;

public class OpenSkillcheck : OpenMinigame
{
    private Skillcheck skillcheck = null;

    public override IEnumerator StartMinigame() {
        // Instantiates the minigame if there is not one running
        if (minigamePrefab != null && currentMinigameInstance == null) {
            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            skillcheck = currentMinigameInstance.GetComponent<Skillcheck>();

            yield return StartCoroutine(skillcheck.CheckSkillcheck());

            isMinigameSuccessful = skillcheck.isMinigameSuccessful;
        }

    }

}
