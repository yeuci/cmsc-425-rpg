using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsWindow : MonoBehaviour
{
    public GameObject window;
    public TMP_Text xpText, lvlText, spText;
    public Image progressBar;
    GameObject levelChanger;

    bool next = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // window.transform.localScale = new Vector3(0, 1, 1);
        progressBar.fillAmount = 0;
        spText.text = "";
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) next = true;   
    }

    public IEnumerator showVictory(int prevLvl, int prevXP, int prevCap, float xpToAdd) {
        window.SetActive(true);

        lvlText.text = "Level: " + prevLvl;
        xpText.text = "XP: " + prevXP + "/" + prevCap;
        progressBar.fillAmount = (float)prevXP/prevCap;
        float addedXP = 0;
        int currXP = prevXP, currCap = prevCap;
        while(addedXP < xpToAdd) {
            addedXP += xpToAdd * Time.deltaTime / 3;
            currXP = (int)Mathf.Round(addedXP + prevXP);
            xpText.text = "XP: " + currXP + "/" + currCap;
            progressBar.fillAmount = (float)currXP/currCap;
            if(currXP >= currCap) {
                progressBar.fillAmount = 0;
                prevXP = 0;
                xpToAdd -= addedXP;
                addedXP = 0;
                currCap *= 2;

                lvlText.text = "Level UP! " + prevLvl +" -> " + (++prevLvl); 
                spText.text = "Skill Points Acquired! " + 1 + " -> " + 2; // add actual skill point amt
            }

            yield return null;
        }

        yield return new WaitUntil(() => next);

        SceneTransition transition = levelChanger.GetComponent<SceneTransition>();
        transition.animator = levelChanger.GetComponent<Animator>();
        yield return StartCoroutine(transition.PlayCombatFinishedTransition());
        
        SceneManager.LoadScene("DungeonMap");
    }
}
