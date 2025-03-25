using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public GameObject looker;

    LookAtThis lookAtThis;

    void Start()
    {
        lookAtThis = looker.GetComponent<LookAtThis>();
    }

    private void OnMouseDown()
    {
        lookAtThis.LookAt(transform);
    }
}
