using System.Collections;

public class OpenButtonMash : OpenMinigame
{
    public int countNeeded;
    public int stepSize;
    public int timeLimit;

    private ButtonMash buttonMash;

    public override IEnumerator StartMinigame() {
        // Instantiates the minigame if there is not one running
        if (minigamePrefab != null && currentMinigameInstance == null) {    
            
            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            buttonMash = currentMinigameInstance.GetComponent<ButtonMash>();

            // Can change number of buttons and the time limit
            buttonMash.countNeeded = countNeeded;
            buttonMash.timeLimit = timeLimit;
            buttonMash.stepSize = stepSize;

            yield return StartCoroutine(buttonMash.StartButtonMash());

            isMinigameSuccessful = buttonMash.isMinigameSuccessful;
        }

    }
}
