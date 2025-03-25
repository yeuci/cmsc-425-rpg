using UnityEngine;

public class Spin : MonoBehaviour
{
    public float dps = 30;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, dps * Time.deltaTime, 0);
    }
}
