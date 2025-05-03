using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;
    public Entity enemyEntityPrefab;
    public Animator fadeAnimator;
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

        EncounterTransition transition = gameObject.AddComponent<EncounterTransition>();
        transition.animator = fadeAnimator;
        yield return StartCoroutine(transition.PlayTransition());

        SceneManager.LoadScene("Scenes/CombatManagerScene");
    }   
}
