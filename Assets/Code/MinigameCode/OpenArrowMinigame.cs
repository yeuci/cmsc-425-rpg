using System.Threading;
using UnityEngine;

public class OpenArrowMinigame : MonoBehaviour
{

    // Takes in the Prefab of the minigame
    public GameObject minigamePrefab; 
    // The Canvas where the minigame will show up
    public Canvas canvas;
    public int sequenceLength;
    public int timeLimit;

    // Current instance of the minigame
    private GameObject currentMinigameInstance = null;
    // Allows us to modify the public fields of the minigame
    private InputSequenceManager inputSequenceManager = null;

    void Start()
    {
        
    }

    public void StartMinigame() {
        // Instantiates the minigame if there is not one running
        if (minigamePrefab != null && currentMinigameInstance == null) {

            currentMinigameInstance = Instantiate(minigamePrefab, canvas.transform);
            inputSequenceManager = currentMinigameInstance.GetComponent<InputSequenceManager>();

            // Can change number of buttons and the time limit
            inputSequenceManager.sequenceLength = sequenceLength;
            inputSequenceManager.timeLimit = timeLimit;
        }
    }

    void Update()
    {            
        // Debug.Log(currentMinigameInstance);
        // Debug.Log(inputSequenceManager.isMinigameSuccessful);
    }
}
