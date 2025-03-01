using UnityEngine;
using System.Collections;
 
public class CameraRotation : MonoBehaviour {
 
    public Transform player;
    public float rotationSpeed = 5f;
    private Vector3 offset;
    private bool isRotating = false;
    private float currentRotationY = 0f;

    void Start()
    {
        offset = new Vector3(0, 3, -10);
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
        transform.LookAt(player);
    }
}