using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class FireballExplosion : MonoBehaviour {

    Vector3 scaleFactor = new Vector3(0f,0f,0f);
    Vector3 movement = new Vector3(0f,0f,0f);
    public Entity target;
    Transform child;
    int loopCount = 0;
    void Start()
    {
        transform.LookAt(target.transform);
        movement = target.transform.position - transform.position;
         child = transform.Find("BallCenter");
         scaleFactor = child.localScale;
         scaleFactor*=0.1f;
         StartCoroutine(Explode());
    }

    float GetZMove() {
        float deltaX = Mathf.Pow(target.transform.position.x-transform.position.x,2);
        float deltaZ = Mathf.Pow(target.transform.position.z-transform.position.z,2);

        return Mathf.Sqrt(deltaX+deltaZ);
    }

    IEnumerator Explode() { 
        transform.Translate(0f,-transform.position.y,GetZMove());
        while(loopCount < 50) {
            child.localScale+=scaleFactor;
            loopCount+=1;
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(gameObject);
        yield return null;
    }
}