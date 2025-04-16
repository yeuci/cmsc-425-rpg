using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//Class Description: This is meant to be attached to a dart object for a dart trap.
public class DartLaunch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Remember that the child's parent is the dart panel
    GameObject target;
    Vector3 direction;
    void Start()
    {
        //This is set up as a child of the pressure plate
        transform.localPosition = new Vector3(0,0.064000003f,-0.00487999432f);
        transform.localEulerAngles = new Vector3(-90,180,90);
        transform.localScale = new Vector3(0.25f,0.25f,4f);
        target = transform.parent.GetChild(0).gameObject;
        Debug.Log("Target: "+target);
        direction = target.transform.position - transform.position;
        direction.y = 0;
        transform.position += direction;
    }

    // Update is called once per frame
    IEnumerator Launch()
    {
        
        yield return null;
    }
}
