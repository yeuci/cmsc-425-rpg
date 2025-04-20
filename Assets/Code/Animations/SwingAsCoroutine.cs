using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//Credit to Professor Stevens for the original Sinusoidal Swing code and idea to make it a coroutine
//I (Jason Altneu) upgraded the original sinusoidal swing and implemented it as a coroutine
public class SwingAsCoroutine : MonoBehaviour
{
    public float frequency = 1; //Start to Start takes frequency seconds
    public float degrees = 30;  //Maximum rotation is degrees
    public float speed = 1;     //Animation takes speed seconds to finish
    public float startPhase = 0;//What point of the Sin curve does the animation start at
    public float endPhase = 1;  //What point of the Sin curve does the animatiion end at
    public float animStart = 0; //How deep into frequency does the animation start

    Quaternion baseRot;
    float start;

    float effectiveTime = 0;
    private PlayerManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();

        baseRot = transform.localRotation;
        start = degrees*Mathf.Sin(startPhase * 2 * Mathf.PI);
        StartCoroutine(Swing());
    }

    // Update is called once per frame
    

    bool KeyPressed(Key k) {
        return Keyboard.current[k].isPressed;
    }

    bool MoveKeyPressed() {
        bool pressed = KeyPressed(Key.W) || KeyPressed(Key.A) || KeyPressed(Key.D) || KeyPressed(Key.S) ||
                KeyPressed(Key.UpArrow) || KeyPressed(Key.DownArrow) || KeyPressed(Key.LeftArrow) ||
                KeyPressed(Key.RightArrow);
        return pressed && !playerManager.isMenuActive;
    }

    IEnumerator Swing() {
        yield return new WaitUntil(() => MoveKeyPressed());

        effectiveTime += Time.deltaTime;
            effectiveTime %= frequency;
            var animTime = 0f;
            if(effectiveTime >= animStart && effectiveTime <= (animStart + speed)) { //If I'm within the animation cycle...
                animTime = effectiveTime - animStart;
                transform.localRotation = baseRot * Quaternion.Euler(0,
                    degrees * Mathf.Sin((startPhase + (endPhase-startPhase)/speed*animTime) * 2 * Mathf.PI), 0);
            }

        StartCoroutine(Swing());
    }
}
