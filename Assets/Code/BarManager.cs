using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//This only works for health bar right now. We haven't limited magic yet, but I will eventually
    // need to extrapolate.
public class BarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity player;
    public Image healthBar;
    public Image manaBar;

    IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate(); // Wait for one frame
        player = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<Entity>();
    }

    void Start()
    {
        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    //Bar Size is set
    void Update()
    {
        Debug.Log(manaBar.fillAmount);
        healthBar.fillAmount = player.remainingHP/player.getAdjustedStats().health;
        manaBar.fillAmount = player.remainingMP/player.getAdjustedStats().mana;
    }
}
