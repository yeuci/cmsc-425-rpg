using UnityEngine;
using System.Collections;


public abstract class OpenMinigame : MonoBehaviour
{
    public GameObject minigamePrefab; // Prefab of the minigame
    public Canvas canvas; // Canvas where the minigame will show up
    public bool isMinigameSuccessful; // Result of the minigame

    protected GameObject currentMinigameInstance = null; // Current instance of the minigame

    // Abstract method to be implemented by child classes
    public abstract IEnumerator StartMinigame();
}
