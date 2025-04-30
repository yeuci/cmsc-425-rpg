using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public ScrollRect scrollRect;

    public GameObject scrollClickBlocker;

    bool blockerActive;

    void Update()
    {
        if (blockerActive == false && Mathf.Abs(scrollRect.velocity.y) > 40f)
        {
            blockerActive = true;
            scrollClickBlocker.SetActive(true);
        }
        else if (
            blockerActive == true && Mathf.Abs(scrollRect.velocity.y) <= 40f
        )
        {
            blockerActive = false;
            scrollClickBlocker.SetActive(false);
        }
    }
}
