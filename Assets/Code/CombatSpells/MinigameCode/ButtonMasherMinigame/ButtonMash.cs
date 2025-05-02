using System.Collections;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonMash : MonoBehaviour
{
    public GameObject buttonMashPanel;  // Panel that holds minigame
    public Image progressBar;           // How much progress player has made
    public Button spaceButton;          // Space bar so it can be animated
    private Animator animator;          // Animator that animates the space bar

    public float countNeeded;           // Count needed to pass minigame
    float currentCount = 0;             // How much count the player has
    public float stepSize = 20;         // How much the player gains when they hit space
    
    public float timeLimit = 5f;        // Time limit
    public TextMeshProUGUI timerText;   // Displays the timer

    public Key spaceKey = Key.Space;    // Key to hit

    public bool isMinigameSuccessful;   // If minigame is successful

    void Start()
    {
        // Gets the animator
        animator = spaceButton.GetComponent<Animator>();
    }


    public IEnumerator StartButtonMash() {
        progressBar.fillAmount = 0;

        float timer = timeLimit; // Start countdown
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        // Run while there is still time and not passed
        while (currentCount < countNeeded && timer > 0) {

            // Is space is pressed
            if (Keyboard.current[spaceKey].wasPressedThisFrame) {
                // Plays animation
                animator.Play("Pressed", -1, 0f);
                // Adds to current count
                currentCount += stepSize;
                spaceButton.onClick.Invoke();
            } 

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                animator.Play("Normal", -1, 0f);
            }


            // If minigame is successful
            if (currentCount >= countNeeded) {
                progressBar.fillAmount = 1;
                isMinigameSuccessful = true;
                yield return new WaitForSeconds(1);
                Destroy(buttonMashPanel);

                yield break;
            }

            // Decrements a finxed amount every frame
            currentCount -= 1.2f * 60f * Time.deltaTime;
            // Clamps current count
            currentCount = Mathf.Clamp(currentCount, 0, countNeeded);

            // Updating fill amount to show how much the player has progressed
            progressBar.fillAmount = Mathf.Clamp(currentCount / countNeeded, 0, 1);

            // Decrement time
            timer -= Time.deltaTime; // Decrease time
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null; // Wait until next frame
        }

        // Wait one second before minigame ends
        yield return new WaitForSeconds(1f);
        // Destroy panel
        Destroy(buttonMashPanel);
        isMinigameSuccessful = false;

        yield break;
    }


}
