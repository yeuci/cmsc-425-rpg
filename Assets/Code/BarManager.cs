using UnityEngine;
using UnityEngine.UI;

//This only works for health bar right now. We haven't limited magic yet, but I will eventually
    // need to extrapolate.
public class BarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity player;
    RectTransform barImage;
    Vector3 barSize;
    void Start()
    {
        barImage = GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<Entity>();
        barSize = barImage.localScale;
    }

    // Update is called once per frame
    //Bar Size is set
    void Update()
    {
        barSize = new Vector3(player.remainingHP/player.getAdjustedStats().health, barSize.y, barSize.z);
        barImage.localScale = barSize;
    }
}
