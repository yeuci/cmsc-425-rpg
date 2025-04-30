using UnityEngine;
using UnityEngine.UI;

public class ArrowMover : MonoBehaviour
{
    public float speed = 150f; // Adjust to match timing window
    public RectTransform targetZone; // Assign the hollow arrow's RectTransform
    float parryWindow = 8f;
    bool inParryWindow = false;
    private Image image;
    public bool running = true;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {   
        // Only moves while running
        if (running) {
            transform.position += Vector3.left * speed * Time.deltaTime;

            // Check if in parry window
            float distance = Mathf.Abs(transform.position.x - targetZone.position.x);
            inParryWindow = distance <= parryWindow; 
        }
    }

    // Checks if arrow is within += 8 frames of the target zone
    public bool CheckParry() {
        return inParryWindow;
    }

    // Returns if arrow has passed target area
    public bool MissedWindow() {
        return transform.position.x <= targetZone.position.x - 15f;
    }

    // Change color of arrow jey
    public void SetColor(Color color)
    {
        if (image != null)
            image.color = color;
    }
}