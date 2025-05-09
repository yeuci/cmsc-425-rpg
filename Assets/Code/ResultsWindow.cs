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
        
    }

    public IEnumerator showVictory(int prevSP, int prevLvl, int prevXP, int prevCap, float xpToAdd) {
        window.SetActive(true);

        lvlText.text = "Level: " + prevLvl;
        xpText.text = "XP: " + prevXP + "/" + prevCap;
        progressBar.fillAmount = (float)prevXP/prevCap;
        float addedXP = 0;
        int currXP = prevXP, currCap = prevCap;
        while(addedXP < xpToAdd) {
            addedXP += xpToAdd * Time.deltaTime / 3;
            currXP = (int)Mathf.Floor(addedXP + prevXP);
            xpText.text = "XP: " + currXP + "/" + currCap;
            progressBar.fillAmount = (float)currXP/currCap;
            if(currXP >= currCap) {
                progressBar.fillAmount = 0;
                prevXP = 0;
                xpToAdd -= addedXP;
                addedXP = 0;
                currCap *= 2;

                lvlText.text = "Level UP! " + prevLvl +" -> " + (++prevLvl); 
                int nextSP = prevSP + 5;
                spText.text = "Skill Points Acquired! " + prevSP + " -> " + nextSP; // add actual skill point amt
                prevSP = nextSP;
            }

            yield return null;
        }

        yield return new WaitUntil(() => next);

        FadeTransition transition = levelChanger.GetComponent<FadeTransition>();
        transition.animator = levelChanger.GetComponent<Animator>();
        yield return StartCoroutine(transition.PlayFadeOutFast());
        
        SceneManager.LoadScene("DungeonMap");
    }

    public void Continue() {
        next = true;
    }
}
