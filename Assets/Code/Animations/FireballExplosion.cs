using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class FireballExplosion : MonoBehaviour {

    Vector3 scaleFactor = new Vector3(0f,0f,0f);
    Vector3 movement = new Vector3(0f,0f,0f);
    GameObject targetObj;
    public Entity target;
    Transform child;
    int loopCount = 0;
    void Start()
    {
        if(target == null) {
            targetObj = GameObject.FindGameObjectWithTag("Enemy");
            target = targetObj.GetComponent<Entity>();
        }
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

    public void setTarget(Entity e) {
        target = e;
    }

    public IEnumerator Explode() { 
        float zMove = GetZMove();
        float yStart = transform.position.y;
        float frames = 50f;
        //yield return new WaitUntil(() => Keyboard.current[Key.I].isPressed);
        for(int i = 1; i <= frames; i++){
            transform.Translate(0f,-(yStart/frames),(zMove/frames));
            yield return new WaitForSeconds(.75f/frames);
        }
        
        //yield return new WaitUntil(() => Keyboard.current[Key.Q].isPressed);
        while(loopCount < 50) {
            child.localScale+=scaleFactor;
            loopCount+=1;
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(gameObject);
        yield return null;
    }
}