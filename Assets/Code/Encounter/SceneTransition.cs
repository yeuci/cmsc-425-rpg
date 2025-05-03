using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    AudioSource encounterSound;
    GameObject musicManager;
    [HideInInspector] public bool isFadingOut;
    public Animator animator;


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

    public IEnumerator PlayCombatFinishedTransition() {
        isFadingOut = true;

        Debug.Log("PLAYING THE TRANSITION");

        animator.SetTrigger("FadeOutFast");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("FadeOutFast"));

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        isFadingOut = false;
    }
}
