using UnityEngine;

public class Swing : MonoBehaviour
{
    public float frequency = 1; //seconds/rotation
    public float degrees = 60; //Maximum degree of swing
    public float phase = 0; //Start point

    Quaternion baseRot;
    float start;

    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.localRotation;
        start = degrees * Mathf.Sin(phase * 2 * Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = baseRot * Quaternion.Euler(0,
            degrees * Mathf.Sin((phase + frequency * Time.time) * 2 * Mathf.PI) - start, 0);
    }
}
