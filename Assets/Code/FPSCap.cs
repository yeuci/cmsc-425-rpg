using UnityEngine;

[ExecuteInEditMode]
public class FPSCap : MonoBehaviour
{
    [SerializeField] private int fpsCap = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsCap;
        #endif
    }

}
