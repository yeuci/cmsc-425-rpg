using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//This only works for health bar right now. We haven't limited magic yet, but I will eventually
    // need to extrapolate.
public class BarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity player;
    PlayerManager playerState;
    public Image healthBar;
    public Image manaBar;

    IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate(); // Wait for one frame
        player = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<Entity>();
        playerState = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
    }

    void Start()
    {
        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    //Bar Size is set
    void Update()
    {
        // Debug.Log(manaBar.fillAmount);
        if (playerState != null && playerState.playerCanCollide) {
            healthBar.fillAmount = player.remainingHP/player.maximumHP;
            manaBar.fillAmount = player.remainingMP/player.maximumMP;
        }
    }
}
