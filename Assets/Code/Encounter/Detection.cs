using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;

    void Start()
    {
        detector = gameObject.AddComponent<SphereCollider>();
        detector.isTrigger = true;
        detector.radius = 3;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log("Contact Made");
            SceneManager.LoadScene("Scenes/CombatManagerScene");
        }
    }
}
