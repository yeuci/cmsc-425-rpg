using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireballExplosion : MonoBehaviour {

    int routineRun = 0;
    Vector3 scaleFactor = new Vector3(0f,0f,0f);
    void Start()
    {
        scaleFactor = transform.localScale;
        scaleFactor*=.4f;
        StartCoroutine(Explode());   
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(1f);
        while(routineRun < 10) {
            transform.localScale+=scaleFactor;
            routineRun++;
            yield return new WaitForSeconds(0.01f);
        }
        if(transform.parent.gameObject != null) {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
        yield return null;
    }
}