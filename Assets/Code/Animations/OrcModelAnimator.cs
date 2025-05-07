using UnityEngine;

public class OrcModelAnimator : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
       animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return;
        }
    }

    void Update()
    {
        // playDeath if k is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            playDeath();
        }
        // playClaw if l is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            playClaw();
        }
        // playMagic if m is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            playMagic();
        }
    }

    public void playDeath() {
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
    }

    public void playClaw() {
        if (animator != null)
        {
            animator.SetTrigger("useClaw");
        }
    }

    public void playMagic() {
        if (animator != null)
        {
            animator.SetTrigger("useMagic");
        }
    }
}
