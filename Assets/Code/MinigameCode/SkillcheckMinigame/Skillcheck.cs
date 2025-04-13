using System.Collections;
using UnityEngine.InputSystem;

using UnityEngine;

public class Skillcheck : MonoBehaviour
{
    public GameObject skillcheckPanel;
    public RectTransform needle;
    public RectTransform successZone;

    public float rotationSpeed = 180f;
    public Key spaceKey = Key.Space;

    private bool isRotating = true;
    public bool isMinigameSuccessful;

    public IEnumerator CheckSkillcheck() {
        float angle = Random.Range(20, 170);

        successZone.eulerAngles = new Vector3(0, 0, angle);
        Debug.Log("STARTING");
        yield return new WaitForSeconds(1);

        float totalRotation = 0;
        isRotating = true;

        float successZoneStartAngle = 360f - angle; // Starting angle of the success zone
        float successZoneFillAngle = 360f * 0.05f; 
        float successZoneEndAngle = (successZoneStartAngle + successZoneFillAngle) % 360f;

        Debug.Log($"Success Zone Start: {successZoneStartAngle}, End: {successZoneEndAngle}");

        while (isRotating) {
            float rotationThisFrame = -rotationSpeed * Time.deltaTime;
            needle.Rotate(0f, 0f, rotationThisFrame);
            totalRotation += Mathf.Abs(rotationThisFrame);


            // Check if the needle overlaps with the success zone
            if (Keyboard.current[spaceKey].wasPressedThisFrame) {
                Debug.Log("PRESSED SPACE");
                if (totalRotation >= successZoneStartAngle && totalRotation <= successZoneEndAngle) {
                    Debug.Log("SUCCESS: Needle is in the success zone!");
                    isMinigameSuccessful = true;
                    isRotating = false;

                    yield return new WaitForSeconds(1f);

                    Destroy(skillcheckPanel);
                    yield break;
                } else {
                    Debug.Log("FAILED: Needle is outside the success zone...");
                    isMinigameSuccessful = false;
                    isRotating = false;

                    yield return new WaitForSeconds(1f);

                    Destroy(skillcheckPanel);
                    yield break;
                }


            }

            if (totalRotation >= 360f) {
                isRotating = false;
                Debug.Log("Needle completed one full rotation.");
                isMinigameSuccessful = false;
                yield return new WaitForSeconds(0.5f);

                Destroy(skillcheckPanel);

                yield break;
            }

            yield return null;
        }

        isMinigameSuccessful = false;

        yield break;
    }
}

