using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "BattleAction/Fireball")]
public class Fireball : Action
{
    public GameObject fireball; 
    Transform ftransform;
    GameObject instance;
    Vector3 scaleFactor = new Vector3(0f,0f,0f);
    Vector3 movement = new Vector3(0f,0f,0f);
    GameObject targetObj;
    GameObject target;
    Transform child;
    int loopCount = 0;

    public override void Execute(AnimationManager am, GameObject attacker, GameObject defender) {
        base.Execute(am, attacker, defender);

        instance = Instantiate(fireball, attacker.transform.position, attacker.transform.rotation);
        if(target == null) {
            target = defender;
        }
        ftransform = instance.transform;
        ftransform.LookAt(target.transform);
        movement = target.transform.position - ftransform.position;
        ftransform.localScale*=0.5f;
        child = ftransform.Find("BallCenter");
        scaleFactor = child.localScale;
        scaleFactor*=0.1f;
        am.StartCoroutine(Explode());
        Debug.Log("Fireball!");
    }

    float GetZMove() {
        float deltaX = Mathf.Pow(target.transform.position.x-ftransform.position.x,2);
        float deltaZ = Mathf.Pow(target.transform.position.z-ftransform.position.z,2);

        return Mathf.Sqrt(deltaX+deltaZ);
    }

    public void setTarget(GameObject e) {
        target = e;
    }

    public IEnumerator Explode() { 
        float zMove = GetZMove();
        float yStart = ftransform.position.y;
        float frames = 50f;
        //yield return new WaitUntil(() => Keyboard.current[Key.I].isPressed);
        for(int i = 1; i <= frames; i++){
            ftransform.Translate(0f,-yStart/frames,zMove/frames);
            yield return new WaitForSeconds(.75f/frames);
        }
        
        //yield return new WaitUntil(() => Keyboard.current[Key.Q].isPressed);
        while(loopCount < 50) {
            child.localScale+=scaleFactor;
            loopCount+=1;
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(instance);
        yield return null;
    }
}
