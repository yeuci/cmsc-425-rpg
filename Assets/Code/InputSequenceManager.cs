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
    void Start()
    {
        // Instantiate each key
        up = Keyboard.current[upKey];
        left = Keyboard.current[leftKey];
        down = Keyboard.current[downKey];
        right = Keyboard.current[rightKey];

        StartCoroutine(StartMinigame());
    }

    IEnumerator StartMinigame()
    {

        CreateArrowSlots(sequenceLength);
        GenerateRandomSequence(sequenceLength);
        DisplaySequence();


        float timer = timeLimit; // Start countdown
        while (currentStep < requiredSequence.Count && timer > 0)
        {
            if (CheckInput()) // Check input every frame
            {
                if (currentStep == requiredSequence.Count)
                {
                    Debug.Log("Sequence Completed!");
                    arrowMinigamePanel.SetActive(false);
                    yield break; // End coroutine
                }
            }
            timer -= Time.deltaTime; // Decrease time
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null; // Wait until next frame
        }

        Debug.Log("Time's Up! Restarting...");        
        // Reset sequence and current step
        yield return new WaitForSeconds(1);
        ResetSequence();
        currentStep = 0; // Reset current step
        StartCoroutine(StartMinigame()); // Restart the minigame coroutine
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
                Debug.Log("Wrong Input! Restarting...");
                arrowSlots[currentStep].color = Color.red;
                ResetSequence();
                return false;
            }
            return false;
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

        // Resets game
        void ResetSequence()
        {
            currentStep = 0;
            foreach (Image arrow in arrowSlots)
            {
                arrow.color = Color.white; // Reset color
            }
        }
}
