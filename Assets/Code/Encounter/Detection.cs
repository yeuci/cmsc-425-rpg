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

            // TODO!! layer scene on top of current scene and unload combatmanagerscene after battle ends
            SceneManager.LoadScene("Scenes/CombatManagerScene");
        }
    }
}
