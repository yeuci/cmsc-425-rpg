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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log("Contact Made");
            SceneManager.LoadScene("Scenes/CombatManagerScene");
        }
    }
}
