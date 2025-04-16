using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//Class Description: This is meant to be attached to a dart object for a dart trap.
public class DartLaunch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        //This is set up as a child of the pressure plate
        transform.position = new Vector3(128.830002f,1.28000069f,167.853668f);
        transform.localEulerAngles = new Vector3(270,270,0);
        transform.localScale = new Vector3(5,0.200000048f,0.200000077f);
        
    }

    // Update is called once per frame
    IEnumerator Launch()
    {
        
        yield return null;
    }
}
