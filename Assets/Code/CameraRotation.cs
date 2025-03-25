using UnityEngine;
using System.Collections;
 
public class CameraRotation : MonoBehaviour {
 
    public Transform player;
    public float rotationSpeed = 8f;
    private Vector3 offset;
    private bool isRotating = false;
    private float currentRotationY = 0f;

    void Start()
    {
        offset = new Vector3(0, 8, -10);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            currentRotationY += mouseX * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(0, currentRotationY, 0);
        transform.position = player.position + rotation * offset;
        player.eulerAngles = new Vector3(player.eulerAngles.x, currentRotationY+90, player.eulerAngles.z);
        transform.LookAt(player);
    }
}