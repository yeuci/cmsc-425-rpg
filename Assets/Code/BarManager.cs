using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Entity player;
    RectTransform barImage;
    Vector3 barSize;
    void Start()
    {
        barImage = GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        barSize = barImage.localScale;
    }

    // Update is called once per frame
    //Bar Size is set
    void Update()
    {
        float percentHealthRemaining = player.remainingHP/player.getAdjustedStats().health;
        Debug.Log(percentHealthRemaining);
        barSize = new Vector3(player.remainingHP/player.getAdjustedStats().health, barSize.y, barSize.z);
        barImage.localScale = barSize;

        Debug.Log("Right: "+barImage.right);
        
    }
}
