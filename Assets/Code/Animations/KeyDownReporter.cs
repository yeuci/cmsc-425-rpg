using System.Collections;
using UnityEngine;

public class KeyDownReporter : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReportKeyDown());
    }

    IEnumerator ReportKeyDown()
    {
        int n = 0;

        WaitUntil pause = new WaitUntil( () => Input.anyKeyDown);

        while (true)
        {
            Debug.Log(n);

            n = n + 1;

            if (n == 10)
            {
                yield break;
            }

            yield return pause;
        }
    }
}
