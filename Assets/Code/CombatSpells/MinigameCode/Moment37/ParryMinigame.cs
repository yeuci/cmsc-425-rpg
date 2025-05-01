using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParryMinigame : MonoBehaviour
{
    public GameObject arrowPrefab;          // Prefab of arrow to be spawed
    public GameObject parryMinigamePanel;   // Panel that holds minigame
    public RectTransform spawnPoint;        // Where arrows spawn
    public RectTransform parryZone;         // Zone where valid parry is

    public int batchOne, batchTwo = 7;      // Arrows will appear in three total batches
    public int totalParries = 15;           // 15 arrows in total to hit
    public float interval = 0.33f;          // Interval between spawning arrows inside batches
    public Key parryKey = Key.D;            // Key to parry

    // Hold all active arrows. Arrows are created in order and will be added in order
    private List<ArrowMover> activeArrows = new List<ArrowMover>();
    int currentArrow = 0;                   // Hold where current arrow is

    // Holds if minigame is successful
    public bool isMinigameSuccessful = false;
    bool running = true;                    // Tells if minigame isrunning

    void LateUpdate() {
        // If not arrows have been instantiated, return
        if (currentArrow >= activeArrows.Count) return;

        // Gets the arrow that needs to be parried
        ArrowMover arrow = activeArrows[currentArrow];

        // Missed window
        if (arrow != null && arrow.MissedWindow())
        {
            // Set running to false
            running = false;
            // Stops all arrows from advancing
            StopAllArrows();
            // Sets arrow color to red
            arrow.SetColor(Color.red);
            isMinigameSuccessful = false;
            return;
        }

        // Check input
        if (Keyboard.current[parryKey].wasPressedThisFrame)
        {
            if (arrow != null)
            {
                // Checks if parry was successful
                if (arrow.CheckParry())
                {
                    // Destroys arrow and sets position in activeArrows to null
                    Destroy(arrow.gameObject);
                    activeArrows[currentArrow] = null;
                }
                // If parry is missed
                else
                {
                    running = false;
                    StopAllArrows();
                    isMinigameSuccessful = false;
                    arrow.SetColor(Color.red);
                    return;
                }
            }
            // Increment currentArrow when parryKey is pressed
            currentArrow++;
        }

       // If all arrows have been parried
        if (currentArrow == totalParries) {
            running = false;
            isMinigameSuccessful = true;
        }
    }

    // Instantiates arrows to be parried
    public IEnumerator ParrySequence() {

        // Waiting before arrows start coming
        yield return new WaitForSeconds(1);

        // All arrows for first batch
        for (int i = 0; i < batchOne; i++) {
            // Only instantiate arrows if game is running
            if (running) {
                // Initates arrow at spawn postition
                GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity, transform);
                // Set the target zone for the arrow
                var mover = arrow.GetComponent<ArrowMover>();
                mover.targetZone = parryZone;

                // Add to array
                activeArrows.Add(mover);

                yield return new WaitForSeconds(interval);  
            } else {
                break;
            }
        }

        // If game is still running, add small gap before second batch
        if (running) {
            yield return new WaitForSeconds(0.2f);
        }

        // Same logic for second batch
        for (int i = 0; i < batchTwo; i++)
        {
            if (running) {
                GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity, transform);
                var mover = arrow.GetComponent<ArrowMover>();
                mover.targetZone = parryZone;
                activeArrows.Add(mover);
                yield return new WaitForSeconds(interval);  
            } else {
                break;
            }
        }

        // Add longer gap before final arrow
        if (running) {
            yield return new WaitForSeconds(0.35f);
        }
        
        // Instantiate final arrow
        if (running) {
            GameObject finalArrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity, transform);
            var finalMover = finalArrow.GetComponent<ArrowMover>();
            finalMover.targetZone = parryZone;
            activeArrows.Add(finalMover); 
        }

        // Wait unitl minigame is finished
        while (running) {
            yield return null;
        }

        // Wait one second before finishing
        yield return new WaitForSeconds(1);

        // Destroys panel
        Destroy(parryMinigamePanel);

        yield break;
    }


    // Stops all arrows from moving
    void StopAllArrows() {
        foreach (ArrowMover arrowObj in activeArrows) {
            if (arrowObj != null)
            {
                arrowObj.running = false;
            }
        }
    }
}
