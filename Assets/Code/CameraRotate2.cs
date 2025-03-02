using UnityEngine;

public class CameraRotate2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    private Vector3 offset;
    void Start()
    {
        offset = new Vector3(0, 4, -8);
        transform.rotation = Quaternion.Euler(-30,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
        
        transform.LookAt(player);
    }
}
