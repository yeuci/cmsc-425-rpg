using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireballExplosion : MonoBehaviour {

    public Entity target;
    int routineRun = 0;
    Vector3 scaleFactor = new Vector3(0f,0f,0f);
    Vector3 movement = new Vector3(0,0,0);
    void Start()
    {
        scaleFactor = transform.localScale;
        scaleFactor*=.4f;
        movement = target.transform.position - transform.position;
        
        StartCoroutine(Explode());   
    }

    IEnumerator Explode() {
        //The entire object should be moving before this happens
        transform.parent.Translate(movement);
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