using UnityEngine;
using UnityEngine.UI;

//This only works for health bar right now. We haven't limited magic yet, but I will eventually
    // need to extrapolate.
public class BarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity player;
    Image healthBar;
    void Start()
    {
        healthBar = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<Entity>();
    }

    // Update is called once per frame
    //Bar Size is set
    void Update()
    {
        healthBar.fillAmount = player.remainingHP/player.getAdjustedStats().health;
    }
}
