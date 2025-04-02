using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Button attackButton, defendButton;

    public GameObject fireball;
    Entity target, source;
    GameObject targetObj, sourceObj;
   
   //As of now, this gets the attack button, defend button, and determines the enemy.
    void Start()
    {
        attackButton.onClick.AddListener(AttackListener);
        defendButton.onClick.AddListener(DefendListener);
        targetObj = GameObject.FindWithTag("Enemy");
        target = targetObj.GetComponent<Entity>();
        sourceObj = GameObject.FindWithTag("Player");
        source = sourceObj.GetComponent<Entity>();
        Debug.Log(target);
        Debug.Log(source);
    }

    // Update is called once per frame
    void AttackListener()
    {
        GameObject instance = Instantiate(fireball, source.transform.position,Quaternion.identity);
        
        Debug.Log("Attack Clicked");
    }

    void DefendListener() {
        Debug.Log("Defend Clicked");
    }
}
