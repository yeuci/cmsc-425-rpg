using System.Collections;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    AudioSource encounterSound;
    GameObject musicManager;
    [HideInInspector] public bool isFadingOut;
    public Animator animator;
    public AudioSource sceneMusic;


    void Start()
    {
        encounterSound = gameObject.GetComponent<AudioSource>();
        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
        isFadingOut = false;
    }

    public IEnumerator PlayEncounterTransition()
    {

        isFadingOut = true;
        animator.SetTrigger("FadeOut");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut"));
        musicManager.GetComponent<AudioSource>().Stop();
        encounterSound.Play();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        isFadingOut = false;

        // Then trigger fade or scene load
    }

    public IEnumerator PlayFadeOutFast() {
        isFadingOut = true;

        Debug.Log("PLAYING THE TRANSITION");

        animator.SetTrigger("FadeOutFast");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("FadeOutFast"));

        if (sceneMusic != null) {
            StartCoroutine(FadeAudio.FadeOut(sceneMusic, 1f));
        }
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        isFadingOut = false;
    }
}
