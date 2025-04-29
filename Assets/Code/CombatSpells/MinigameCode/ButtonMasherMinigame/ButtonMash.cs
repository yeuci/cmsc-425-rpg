using System.Collections;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonMash : MonoBehaviour
{
    public GameObject buttonMashPanel;
    public Image progressBar;
    public Button spaceButton;
    private Animator animator;

    public float countNeeded;
    float currentCount = 0;
    public float stepSize = 20;
    
    // Timer
    public float timeLimit = 5f;
    // Displays the timer
    public TextMeshProUGUI timerText;

    public Key spaceKey = Key.Space;

    public bool isMinigameSuccessful;

    void Start()
    {
        animator = spaceButton.GetComponent<Animator>();
    }

    public IEnumerator StartButtonMash() {
        progressBar.fillAmount = 0;

        float timer = timeLimit; // Start countdown
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        while (currentCount < countNeeded && timer > 0) {
            if (Keyboard.current[spaceKey].wasPressedThisFrame) {
                animator.Play("Pressed", -1, 0f);
                currentCount += stepSize;
                spaceButton.onClick.Invoke();
            } 

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                animator.Play("Normal", -1, 0f);
            }

            if (currentCount >= countNeeded) {
                progressBar.fillAmount = 1;
                isMinigameSuccessful = true;
                yield return new WaitForSeconds(1);
                Destroy(buttonMashPanel);

                yield break;
            }

            currentCount -= 0.25f;
            currentCount = Mathf.Clamp(currentCount, 0, countNeeded);

            progressBar.fillAmount = Mathf.Clamp(currentCount / countNeeded, 0, 1);

            timer -= Time.deltaTime; // Decrease time
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            yield return null; // Wait until next frame
        }

        yield return new WaitForSeconds(1f);
        Destroy(buttonMashPanel);
        isMinigameSuccessful = false;

        yield break;
    }


}
