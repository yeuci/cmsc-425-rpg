using System.Threading;
using UnityEngine;
using System.Collections;


public class OpenArrowMinigame : OpenMinigame
{
    public int sequenceLength;
    public int timeLimit;
    public GameObject spellPrefab;
    // Allows us to modify the public fields of the minigame
    private InputSequenceManager inputSequenceManager = null;

    public override IEnumerator StartMinigame() {
        // Instantiates the minigame if there is not one running
        if (minigamePrefab != null && currentMinigameInstance == null) {    
            
            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            inputSequenceManager = currentMinigameInstance.GetComponent<InputSequenceManager>();

            // Can change number of buttons and the time limit
            inputSequenceManager.sequenceLength = sequenceLength;
            inputSequenceManager.timeLimit = timeLimit;

            yield return StartCoroutine(inputSequenceManager.runMinigame());

            isMinigameSuccessful = inputSequenceManager.isMinigameSuccessful;
        }

    }

    void Update()
    {            
        // Debug.Log(currentMinigameInstance);
        // Debug.Log(inputSequenceManager.isMinigameSuccessful);
    }
}
