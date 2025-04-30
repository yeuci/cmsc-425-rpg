using System.Collections;

public class OpenParryMinigame : OpenMinigame
{
    private ParryMinigame  parryMinigame;

     public override IEnumerator StartMinigame() {

        if (minigamePrefab != null && currentMinigameInstance == null) {    
            
            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            parryMinigame = currentMinigameInstance.GetComponent<ParryMinigame>();

            yield return StartCoroutine(parryMinigame.ParrySequence());

            isMinigameSuccessful = parryMinigame.isMinigameSuccessful;
        }
        yield break;
     }
}
