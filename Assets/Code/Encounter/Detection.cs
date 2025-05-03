using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;
    public Entity enemyEntityPrefab;
    GameObject levelChanger;
    [HideInInspector] public PlayerManager playerManager;

    void Start()
    {   
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger");

        detector = gameObject.AddComponent<SphereCollider>();
        detector.isTrigger = true;
        detector.radius = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && playerManager.playerCanCollide && !playerManager.isMenuActive) {
            StartCoroutine(EnterCombat());
        }
    }

    IEnumerator EnterCombat() {
        if (enemyEntityPrefab == null) {
            Debug.Log("MADE ENEMY CONTACT RIGHT AFTER BATTLE SCENE... LIKELY DUE TO GAMEOBJECT NOT DESTROYING FAST ENOUGH");
            yield break;
        }

        Debug.Log("Contact Made");

        playerManager.enemyBeforeCombat = enemyEntityPrefab.enemyId;
        playerManager.enemyPositionBeforeCombat = transform.position;

        // Fades scene to black before changing to combat scene
        SceneTransition transition = levelChanger.GetComponent<SceneTransition>();
        transition.animator = levelChanger.GetComponent<Animator>();
        yield return StartCoroutine(transition.PlayEncounterTransition());

        SceneManager.LoadScene("Scenes/CombatManagerScene");
    }   
}
