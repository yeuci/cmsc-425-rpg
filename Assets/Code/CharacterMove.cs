using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour
{
    GameObject playerObject;
    Entity playerEntity;
    Stat player;
    public float moveSpeed;
    public float jumpSpeed = 6.0f;
    public float mouseSensitivity = 100.0f;
    public float gravity = 9.8f;

    public Transform playerCamera;
    public float cameraPitch = 0f;
    public float pitchClamp = 80f;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        /* playerObject = GameObject.FindGameObjectWithTag("Player");
        playerEntity = playerObject.GetComponent<Entity>();
        player = playerEntity.getAdjustedStats();
        moveSpeed = player.speed;*/
        moveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // This part handles movement and jumping
        MovePlayer();
        // This part handles horizontal rotation
        RotatePlayer();
    }

    void MovePlayer()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Keeps grounded
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        }
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player on Y axis
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera on X axis
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -pitchClamp, pitchClamp);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }
}
