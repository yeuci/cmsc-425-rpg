using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;
    public Entity enemyEntityPrefab;
    [HideInInspector] public PlayerManager playerManager;

    void Start()
    {   
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();

        detector = gameObject.AddComponent<SphereCollider>();
        detector.isTrigger = true;
        detector.radius = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log("Contact Made");

            playerManager.enemyBeforeCombat = enemyEntityPrefab.enemyId;
            playerManager.enemyPositionBeforeCombat = transform.position;

            SceneManager.LoadScene("Scenes/CombatManagerScene");
        }
    }
}
