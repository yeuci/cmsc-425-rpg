using System.Collections;
using UnityEngine;

public class EncounterTransition : MonoBehaviour
{

    public Transform cameraTransform;
    public Animator animator;
    public float zoomDuration = 1f;
    public float targetFOV = 20f;
    public float rotationAngle = 360f;
    private Camera cam;

    public IEnumerator PlayTransition()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<Camera>();
        cameraTransform = cam.transform;
        yield return StartCoroutine(ZoomAndRotate());
    }

    private IEnumerator ZoomAndRotate()
    {
        // float elapsed = 0f;
        // float startFOV = cam.fieldOfView;
        // Quaternion startRot = cameraTransform.rotation;
        // Quaternion endRot = startRot * Quaternion.Euler(0, 0, rotationAngle);

        // while (elapsed < zoomDuration)
        // {
        //     float t = elapsed / zoomDuration;
        //     cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
        //     cameraTransform.rotation = Quaternion.Slerp(startRot, endRot, t);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }

        // cam.fieldOfView = targetFOV;
        // cameraTransform.rotation = endRot;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMove>().enabled = false;
        animator.SetTrigger("Fade_out");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);


        // Then trigger fade or scene load
    }
}
