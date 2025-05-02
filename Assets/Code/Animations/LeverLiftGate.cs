using System.Collections;
using UnityEngine;

public class LeverLiftGate : MonoBehaviour
{
    public Transform leverArm;            // The moving part of the lever
    public Transform portcullisGate;       // The portcullis gate
    public float leverRotateAngle = -45f;  // Degrees the lever moves when pulled
    public float gateLiftHeight = 10f;       // How far the gate rises
    public float interactionDistance = 3f; // How close player must be
    public float openTime = 2f;         // Seconds for gate to rise

    private bool isActivated = false;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player not found. Make sure to tag your player as 'Player'.");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return; // Safeguard

        if (Vector3.Distance(leverArm.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isActivated)
            {
                StartCoroutine(ActivateLeverAndGate());
            }   
        }
    }

    IEnumerator ActivateLeverAndGate() 
    {
        isActivated = true;

        Quaternion startLeverRotation = leverArm.localRotation;
        Quaternion targetLeverRotation = startLeverRotation * Quaternion.Euler(0f, 0f, leverRotateAngle);

        Vector3 startGatePosition = portcullisGate.localPosition;
        Vector3 targetGatePosition = startGatePosition + new Vector3(0f, gateLiftHeight, 0f);

        // Rotate lever
        float elapsed = 0f;
        while (elapsed < openTime)
        {
            leverArm.localRotation = Quaternion.Slerp(startLeverRotation, targetLeverRotation, elapsed / openTime);
            portcullisGate.localPosition = Vector3.Lerp(startGatePosition, targetGatePosition, elapsed / openTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        leverArm.localRotation = targetLeverRotation;
        portcullisGate.position = targetGatePosition;
    }
}
