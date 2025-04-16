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
        
    }

    // Update is called once per frame
    IEnumerator Launch()
    {
        
        yield return null;
    }
}
