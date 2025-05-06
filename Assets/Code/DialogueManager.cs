using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public int currentDisplayedDialogue = 0;

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
        currentDisplayedDialogue++;
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void NextDialogue()
    {
        string[] dialogueList = {
            "p0",
            "p1",
            "p2",
            "p3",
            "p4",
            "p5",
        };
        
        if (currentDisplayedDialogue >= dialogueList.Length)
        {
            dialogueText.text = "p6";
            return;
        }

        if (currentDisplayedDialogue == 4)
        {
            // TODO: SHOW CHEST
        }

        dialogueText.text = dialogueList[currentDisplayedDialogue];
        currentDisplayedDialogue++;
    }

}
