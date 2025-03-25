using System.Collections;
using UnityEngine;

public class WaitForKey : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        Debug.Log("I'm waiting.");

        yield return new WaitUntil(() => Input.anyKeyDown);

        Debug.Log("Well, that's over.");
    }

    bool WaitKey()
    {
        return Input.anyKeyDown;
    }
}
