using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float openTime = 3f;
    public float interactionDistance = 3f;

    private Transform player;
    private Quaternion rotClosed;
    private Quaternion rotOpened;
    private bool isOpening = false;
    private bool isClosed = true;
    private bool isFacingPositive = true;

    private void Start()
    {
        rotClosed = transform.localRotation;

        // Find the player automatically by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject tagged 'Player' found in the scene.");
        }
    }

    private void Update()
    {
        if (player == null) return; // Safeguard

        if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOpening)
            {
                Vector3 doorToPlayer = player.position - transform.position;
                float dot = Vector3.Dot(-transform.forward, doorToPlayer);

                isFacingPositive = dot < 0;

                float angle = isFacingPositive ? openAngle : -openAngle;
                rotOpened = Quaternion.Euler(0, angle, 0) * rotClosed;

                StartCoroutine(OpenDoor());
            }
        }
    }

    IEnumerator OpenDoor()
    {
        isOpening = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = isClosed ? rotOpened : rotClosed;

        float elapsed = 0f;

        while (elapsed < openTime)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed * 3 / openTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
        isClosed = !isClosed;
        isOpening = false;
    }
}