using UnityEngine;

public class Swing : MonoBehaviour
{
    public float frequency = 1; //seconds/rotation
    public float degrees = 60; //Maximum degree of swing
    public float phase = 0; //Start point

    public float animStart = 0;

    public float animEnd = 1;

    Quaternion baseRot;
    float start;

    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.localRotation;
        start = degrees * Mathf.Sin(phase * 2 * Mathf.PI);
        if(animStart < 0) {
            animStart = 0;
        }
        if(animEnd > frequency) {
            animEnd = frequency;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var effectiveTime = Time.time % frequency;
        if(animEnd > animStart) {
            if(animStart <= effectiveTime && effectiveTime <= animEnd) {
                transform.localRotation = baseRot * Quaternion.Euler(0,
                degrees * Mathf.Sin((phase + effectiveTime) * 2 * Mathf.PI) - start, 0);
            }
        } else {
            if(animStart >= effectiveTime || effectiveTime <= animEnd) {
                transform.localRotation = baseRot * Quaternion.Euler(0,
                degrees * Mathf.Sin((phase + effectiveTime) * 2 * Mathf.PI) - start, 0);
            }
        }
    }
}
