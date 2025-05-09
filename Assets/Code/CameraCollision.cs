using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private Transform player; // The player
    public Vector3 offset = new Vector3(0, 8, -10); // Default camera position
    public float rotationSpeed = 5f; // Rotation speed of camera/player
    public float minY = -45f; // Minimum Y degree of camera rotation
    public float maxY = 50f; // Maximum Y degree of camera rotation
    public LayerMask collisionMask;

    public bool openedMenu = false;

    private float currentYaw = 0f;
    private float currentPitch = 10f;
    private float distance;

    private PlayerManager playerManager;
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        distance = offset.magnitude;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (playerManager == null) return;

        if (playerManager.isMenuActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        HandleRotation();
        HandleCameraCollision();
    }

    public void UpdateCameraPosition()
    {
        Debug.Log("UPDATING THE CAMERA");
        HandleCameraCollision();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);
    }

    void HandleCameraCollision()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = player.position + rotation * offset;
        player.eulerAngles = new Vector3(player.eulerAngles.x, currentYaw + 90, player.eulerAngles.z);

        RaycastHit hit;
        Vector3 direction = desiredPosition - player.position;

        if (Physics.Raycast(player.position, direction.normalized, out hit, distance, collisionMask))
        {
            transform.position = hit.point - direction.normalized * 0.3f;
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(player.position + Vector3.up * 2.0f);
    }
}
