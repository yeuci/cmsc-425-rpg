using UnityEngine;

public class ImprovedSwing : MonoBehaviour
{
    public float frequency = 1; //Start to Start takes frequency seconds
    public float degrees = 30;  //Maximum rotation is degrees
    public float speed = 1;     //Animation takes speed seconds to finish
    public float startPhase = 0;//What point of the Sin curve does the animation start at
    public float endPhase = 1;  //What point of the Sin curve does the animatiion end at
    public float animStart = 0; //How deep into frequency does the animation start

    Quaternion baseRot;
    float start;

    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.localRotation;
        start = degrees*Mathf.Sin(startPhase * 2 * Mathf.PI);
        //Do some variable control here
    }

    // Update is called once per frame
    void Update()
    {
        var effectiveTime = Time.time % frequency; //How deep into the cycle am I
        var animTime = 0f;
        if(effectiveTime >= animStart && effectiveTime <= (animStart + speed)) { //If I'm within the animation cycle...
            animTime = effectiveTime - animStart;
            transform.localRotation = baseRot * Quaternion.Euler(0,
                degrees * Mathf.Sin((startPhase + (endPhase-startPhase)/speed*animTime) * 2 * Mathf.PI), 0);
        }
    }
}