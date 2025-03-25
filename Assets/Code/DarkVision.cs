using Unity.VisualScripting;
using UnityEngine;

public class DarkVision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject player;
    public float lightRange = 10f;
    private Light darkvision;
    void Start()
    {
        if (player == null)
    {
        player = this.gameObject; 
    }

    darkvision = gameObject.AddComponent<Light>();
    darkvision.type = LightType.Spot;
    darkvision.color = UnityEngine.Color.gray;
    darkvision.transform.position = player.transform.position + new Vector3(0,0,0);
    darkvision.intensity = 1;
    darkvision.spotAngle = 60;
    darkvision.range = lightRange;
    darkvision.enabled = true;
    }

}
