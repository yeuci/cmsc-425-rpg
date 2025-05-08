using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public int currentDisplayedDialogue = 1;
    public int heroCounter = 0;

    public AudioSource audioSource;
    public AudioClip pingClip;
    public AudioClip[] orcDialogueClips;
    public AudioClip[] heroDialogueClips;

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {

    }

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
        if (currentDisplayedDialogue == 1) {
            audioSource.PlayOneShot(orcDialogueClips[0]);
        }
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void NextDialogue()
    {
        StartCoroutine(PlayDialogueWithDelay(1.5f));
    }

    private IEnumerator PlayDialogueWithDelay(float delay)
    {
        string[] dialogueList = {
            "yurr. what goin",
            "i'm Meatface McMercy twin.",
            "i use to be part of the pack but they bullied me just because i'm a lil different.",
            "of course you are, you ain't the first. i ain't no rat now but i can help you if you want",
            "yeah, i'm tired of the bullying man. you need to loot up if you want a fighting chance, follow the ping i just put behind you twin. got you with a sword and a chug jug.",
            "no problem, give 'em hell out there. and watch out, i might look small, but the other ones.... they the real hitters"
        };

        if (currentDisplayedDialogue >= dialogueList.Length)
        {
            dialogueText.text = "what you still doin here kid, buddy trippin";
            audioSource.PlayOneShot(orcDialogueClips[orcDialogueClips.Length - 1]);
            yield break;
        }

        if (heroCounter < heroDialogueClips.Length && heroDialogueClips[heroCounter] != null)
        {
            audioSource.PlayOneShot(heroDialogueClips[heroCounter]);
            heroCounter++;
        }

        if (heroCounter == 3 || heroCounter == 4) {
            yield return new WaitForSeconds(delay + 1f);
        } else {
            yield return new WaitForSeconds(delay);
        }

        if (currentDisplayedDialogue < orcDialogueClips.Length && orcDialogueClips[currentDisplayedDialogue] != null)
        {
            audioSource.PlayOneShot(orcDialogueClips[currentDisplayedDialogue]);
        }

        dialogueText.text = dialogueList[currentDisplayedDialogue++];

        if (currentDisplayedDialogue == 5)
        {
            // TODO: SHOW CHEST
            yield return new WaitForSeconds(7.5f);
            audioSource.PlayOneShot(pingClip);
        }
    }


    // public void NextDialogue()
    // {
    //     string[] dialogueList = {
    //         "yurr. what goin",
    //         "i'm Meatface McMercy twin.",
    //         "i use to be part of the pack but they bullied me just because i'm a lil different.",
    //         "of course you are, you ain't the first. i ain't no rat now but i can help you if you want",
    //         "yeah, i'm tired of the bullying man. you need to loot up if you want a fighting chance, follow the ping i just put behind you twin. got you with a sword and a chug jug.",
    //         "no problem, give 'em hell out there. and watch out, i might look small, but the other ones.... they the real hitters"
    //     };
        
    //     if (currentDisplayedDialogue >= dialogueList.Length)
    //     {
    //         dialogueText.text = "what you still doin here kid, buddy trippin";
    //         audioSource.PlayOneShot(orcDialogueClips[orcDialogueClips.Length - 1])
    //         return;
    //     }

    //     if (currentDisplayedDialogue == 4)
    //     {
    //         // TODO: SHOW CHEST
    //     }

    //     if (heroCounter < heroDialogueClips.Length && heroDialogueClips[heroCounter] != null)
    //     {
    //         audioSource.PlayOneShot(heroDialogueClips[0]);
    //         heroCounter++;
    //     }

    //     if (currentDisplayedDialogue < orcDialogueClips.Length && orcDialogueClips[currentDisplayedDialogue] != null)
    //     {
    //         audioSource.PlayOneShot(orcDialogueClips[currentDisplayedDialogue]);
    //     }

    //     dialogueText.text = dialogueList[currentDisplayedDialogue++];
    // }

}
