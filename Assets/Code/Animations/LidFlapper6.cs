using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidFlapper6 : MonoBehaviour
{
    public float angleOpened = 110; // X-axis Euler angle when opened.
    public float angleClosed = 0;   // X-axis Euler angle when closed.

    public float flapTime = 3; // Number of seconds to open or close.

    Quaternion rotOpened; // Rotation when fully opened.
    Quaternion rotClosed; // Rotation when full closed.

    // Set this according to whether we are going from zero
    // to one, or from one to zero.

    float changeSign;

    RevCo revCo;

    private void Start()
    {
        // Create and remember the open/closed quaternions.

        rotOpened = Quaternion.Euler(angleOpened, 0, 0);
        rotClosed = Quaternion.Euler(angleClosed, 0, 0);

        revCo = gameObject.AddComponent<RevCo>();
        revCo.Init(FlapLid);
    }

    private void OnMouseDown()
    {
        revCo.Action();
    }

    private void FlapLid(float t)
    {
        // Set the X angle to however much rotation is done so far.

        transform.localRotation = Quaternion.Lerp(rotClosed, rotOpened, t);
    }
}
