using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detector = gameObject.AddComponent<SphereCollider>();
        detector.isTrigger = true;
        detector.radius = 3;

    }

    // Update is called once per frame
    void Update()
    {  
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(other.gameObject);
            SceneManager.LoadScene("Scenes/CombatManagerScene");
        }
    }
}
