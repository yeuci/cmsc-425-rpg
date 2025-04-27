using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float openTime = 3f;
    public float interactDistance = 3f;
    
    private Transform player;
    private Quaternion rotOpened; // Rotation when fully opened.
    private Quaternion rotClosed; // Rotation when full closed.

    private bool isOpening = false; // Animate and lockout while true.
    private bool isClosed = true; // Track open/closed state.
    private bool isFacingPositive = true; // Whether we open + or - depending on player position

    private void Start()
    {
        rotClosed = transform.localRotation;
    }

    private void OnMouseDown()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        // Lockout any attempt to start another OpenDoor while
        // one is already running.

        if (isOpening)
        {
            yield break;
        }

        // Start the animation and lockout others.

        isOpening = true;

        // Vary this from zero to one, or from one to zero,
        // to interpolate between our quaternions.

        float interpolationParameter;

        // Set this according to whether we are going from zero
        // to one, or from one to zero.

        float changeSign;

        // Set lerp parameter to match our state, and the sign
        // of the change to either increase or decrease the
        // lerp parameter during animation.

        if (isClosed)
        {
            interpolationParameter = 0;
            changeSign = 1;
        }
        else
        {
            interpolationParameter = 1;
            changeSign = -1;
        }

        while (isOpening)
        {
            // Change our "lerp" parameter according to speed and time,
            // and according to whether we are opening or closing.

            interpolationParameter = interpolationParameter + changeSign * Time.deltaTime / openTime;

            // At or past either end of the lerp parameter's range means
            // we are on our last step.

            if (interpolationParameter >= 1 || interpolationParameter <= 0)
            {
                // Clamp the lerp parameter.

                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);

                isOpening = false; // Signal the loop to stop after this.
            }

            // Set the X angle to however much rotation is done so far.

            transform.localRotation = Quaternion.Lerp(rotClosed, rotOpened, interpolationParameter);

            // Tell Unity to start us up again at some future time.

            yield return null;
        }

        // Toggle our open/closed state.

        isClosed = !isClosed;
    }
}
