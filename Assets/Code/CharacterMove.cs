using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    GameObject playerObject;
    Entity playerEntity;
    Stat player;
    public float moveSpeed;
    public float jumpSpeed = 6.0f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        
        controller = GetComponent<CharacterController>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerEntity = playerObject.GetComponent<Entity>();
        player = playerEntity.getAdjustedStats();
        moveSpeed = player.speed;
    }

    void Update()
    {
        if (playerManager != null && playerManager.isMenuActive)
        {
            return; 
        }

        isGrounded = controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection.magnitude > 0.1f)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            moveDirection = forward * vertical + right * horizontal;
            moveDirection.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpSpeed;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move((moveDirection * moveSpeed + velocity) * Time.deltaTime);
    }
}
