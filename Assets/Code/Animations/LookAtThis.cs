using System.Collections;
using UnityEngine;

public class LookAtThis : MonoBehaviour
{
    public float degreesPerSecond = 90;
    public float minimumAngle = 1;

    Transform pointerTrans;

    IEnumerator look;

    private void Start()
    {
        pointerTrans = transform.Find("Base").Find("Pointer");
    }

    public void LookAt(Transform trans)
    {
        if (look != null)
        {
            StopCoroutine(look);
        }

        Quaternion rotFrom = pointerTrans.rotation;
        Quaternion rotTo = new Quaternion();
        rotTo.SetLookRotation(trans.position - pointerTrans.position);

        look = Look(rotFrom, rotTo);
        StartCoroutine(look);
    }

    private IEnumerator Look(Quaternion rotFrom, Quaternion rotTo)
    {
        float angle = Quaternion.Angle(rotFrom, rotTo);
        Debug.Log(angle);

        if (angle < minimumAngle)
        {
            pointerTrans.rotation = rotTo;
            look = null;
            yield break;
        }

        float step = degreesPerSecond / angle;

        float t = 0;

        while (t < 1)
        {
            t = t + step * Time.deltaTime;

            if (t > 1)
            {
                t = 1;
            }

            Quaternion rot = Quaternion.Lerp(rotFrom, rotTo, t);

            pointerTrans.rotation = rot;

            yield return null;
        }

        look = null;
    }
}
