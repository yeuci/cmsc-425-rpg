using System.Collections;
using UnityEngine;

public class CountCo : MonoBehaviour
{
    int updateCount = 0;

    private void Update()
    {
        Debug.LogFormat("Update {0} is up to {1}", gameObject.name, updateCount++);
    }

    private void OnMouseDown()
    {
        StartCoroutine(Report());
    }

    IEnumerator Report()
    {
        int reportCount = 0;

        while (true)
        {
            Debug.LogFormat("Report {0} is up to {1}", gameObject.name, reportCount++);

            yield return null;
        }
    }
}
