using System.Collections;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.UI;

public class Skillcheck : MonoBehaviour
{
    public GameObject skillcheckPanel;
    public Image needle;
    public Image successZone;
    public float rotationSpeed = 180f;
    public Key spaceKey = Key.Space;

    private bool isRotating = true;
    public bool isMinigameSuccessful;

    public IEnumerator CheckSkillcheck() {
        float angle = Random.Range(20, 170);

        successZone.transform.eulerAngles = new Vector3(0, 0, angle);
        yield return new WaitForSeconds(1);

        float totalRotation = 0;
        isRotating = true;

        float successZoneStartAngle = 360f - angle; // Starting angle of the success zone
        float successZoneFillAngle = 360f * successZone.fillAmount; 
        float successZoneEndAngle = (successZoneStartAngle + successZoneFillAngle) % 360f;

        while (isRotating) {
            float rotationThisFrame = -rotationSpeed * Time.deltaTime;
            needle.transform.Rotate(0f, 0f, rotationThisFrame);
            totalRotation += Mathf.Abs(rotationThisFrame);


            // Check if the needle overlaps with the success zone
            if (Keyboard.current[spaceKey].wasPressedThisFrame) {
                if (totalRotation >= successZoneStartAngle && totalRotation <= successZoneEndAngle) {
                    Debug.Log("SUCCESS: Needle is in the success zone!");
                    isMinigameSuccessful = true;
                    isRotating = false;
                    needle.color = Color.green;

                    yield return new WaitForSeconds(1f);

                    Destroy(skillcheckPanel);
                    yield break;
                } else {
                    Debug.Log("FAILED: Needle is outside the success zone...");
                    isMinigameSuccessful = false;
                    isRotating = false;
                    needle.color = Color.red;

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

