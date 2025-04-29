using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using TMPro;

public class InputSequenceManager : MonoBehaviour
{
    public GameObject arrowPrefab; // Dynamically create arrows from prefab
    public List<Image> arrowSlots = new List<Image>(); // Assign UI Image slots in the Inspector
    public Transform arrowContainer; // UI Panel that holds the arrows
    public GameObject arrowMinigamePanel; // Parent panel that will get hidden when game is done
    public Sprite upArrow, leftArrow, downArrow, rightArrow; // Images for each arrow
    
    // Defining which key corresponds with which image
    public Key upKey = Key.W;
    public Key leftKey = Key.A;
    public Key downKey = Key.S;
    public Key rightKey = Key.D;

    // Length of button sequence
    public int sequenceLength = 5;
    // Timer
    public float timeLimit = 5f;
    // Displays the timer
    public TextMeshProUGUI timerText;

    // KeyControl to check for imput
    KeyControl up;
    KeyControl left;
    KeyControl down;
    KeyControl right;

    // Required sequence for player to hit
    private List<KeyControl> requiredSequence = new List<KeyControl>();
    // Current key player needs to hit
    private int currentStep = 0;
    public bool isMinigameSuccessful; // Tracks success or failure

    void Start()
    {
        up = Keyboard.current[upKey];
        left = Keyboard.current[leftKey];
        down = Keyboard.current[downKey];
        right = Keyboard.current[rightKey];
    }

    public IEnumerator runMinigame() {
        up = Keyboard.current[upKey];
        left = Keyboard.current[leftKey];
        down = Keyboard.current[downKey];
        right = Keyboard.current[rightKey];
        yield return StartCoroutine(StartMinigame());
    }

    IEnumerator StartMinigame()
    {

        // Instantiates the arrows that will be displayed
        CreateArrowSlots(sequenceLength);
        // Generate random arrow sequence
        GenerateRandomSequence(sequenceLength);
        // Display arrow sequence
        DisplaySequence();

        float timer = timeLimit; // Start countdown
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        yield return new WaitForSeconds(0.5f);

        // Loop while you still have arrows to press and above timer
        while (currentStep < requiredSequence.Count && timer > 0)
        {
            if (CheckInput()) // Check input every frame
            {
                if (currentStep == requiredSequence.Count)
                {
                    Debug.Log("Sequence Completed!");
                    isMinigameSuccessful = true;
                    yield return new WaitForSeconds(1f);
                    Destroy(arrowMinigamePanel);
                    yield break; // End coroutine
                }
            } else {
                yield return new WaitForSeconds(1);
                isMinigameSuccessful = false;
                Destroy(arrowMinigamePanel);
                yield break;
            }
            timer -= Time.deltaTime; // Decrease time
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null; // Wait until next frame
        }

        yield return new WaitForSeconds(1f);
        isMinigameSuccessful = false;
        Destroy(arrowMinigamePanel);
        yield break;
    }

    // Generates a random button sequence
    void GenerateRandomSequence(int length)
    {
        // Clears previous button sequence
        requiredSequence.Clear();
        KeyControl[] possibleKeys = { up, left, down, right };
        
        for (int i = 0; i < length; i++)
        {
            // Adds random button sequence to requiredSequence
            requiredSequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }

    }

    // Creates arrow key images to be displayed
    void CreateArrowSlots(int sequenceLength)
    {
        // Remove old arrows
        foreach (Transform child in arrowContainer)
        {
            Destroy(child.gameObject);
        }

        arrowSlots.Clear();

        // Create new arrows
        for (int i = 0; i < sequenceLength; i++)
        {
            GameObject arrowGO = Instantiate(arrowPrefab, arrowContainer); // Instantiate inside ArrowContainer
            Image arrowImage = arrowGO.GetComponent<Image>();
            arrowSlots.Add(arrowImage);
        }

        // Update layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(arrowContainer.GetComponent<RectTransform>());
    }

    // Displays sequence inside the arrowContainer
    void DisplaySequence()
    {
        for (int i = 0; i < requiredSequence.Count; i++)
        {
            if (i < arrowSlots.Count)
            {
                arrowSlots[i].gameObject.SetActive(true);
                arrowSlots[i].sprite = GetArrowSprite(requiredSequence[i]);
            }
        }
    }

    bool CheckInput()
    {   
        if (requiredSequence[currentStep].wasPressedThisFrame)
        {
            arrowSlots[currentStep].color = Color.green;
            currentStep++;
            return true;
        }
        else if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("Wrong Input! Stopping...");
            arrowSlots[currentStep].color = Color.red;
            return false;
        }
        return true;
    }

    // Grabs the arrow sprites. Can be changed publically
    Sprite GetArrowSprite(KeyControl key)
    {
        if (key == up) return upArrow;
        if (key == down) return downArrow;
        if (key == left) return leftArrow;
        if (key == right) return rightArrow;
        return null;
    }

}
