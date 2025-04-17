using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player; // The player
    public Vector3 offset = new Vector3(0, 8, -10); // Default camera position
    public float rotationSpeed = 5f;
    public float minY = -45f;
    public float maxY = 55f;
    public LayerMask collisionMask;

    public bool inventoryActive = false;

    private float currentYaw = 0f;
    private float currentPitch = 10f;
    private float distance;

    void Start()
    {
        distance = offset.magnitude;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Disable camera rotation if on inventory or in combat
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryActive = !inventoryActive;
        }
        HandleRotation();
        HandleCameraCollision();
    }

    void HandleRotation()
    {
        // Rotate player by cursor coordinates
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // If inventory is open, disable rotation and unlock cursor
        if (inventoryActive)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // If inventory isn't open, enable rotation and lock cursor
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            currentYaw += mouseX;
            currentPitch -= mouseY;
            currentPitch = Mathf.Clamp(currentPitch, minY, maxY);
        }
    }

    void HandleCameraCollision()
    {
        // Desired camera position based on rotation and offset
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = player.position + rotation * offset;
        player.eulerAngles = new Vector3(player.eulerAngles.x, currentYaw + 90, player.eulerAngles.z);

        // Raycast from target to desired position
        RaycastHit hit;
        Vector3 direction = desiredPosition - player.position;

        if (Physics.Raycast(player.position, direction.normalized, out hit, distance, collisionMask))
        {
            // If there's a hit, move camera to that point instead
            transform.position = hit.point - direction.normalized * 0.3f; // offset so it doesn't clip into object
        }
        else
        {
            transform.position = desiredPosition;
        }
        // Always look at the player
        transform.LookAt(player.position + Vector3.up * 2.0f); // Adjust for head height
    }
}
