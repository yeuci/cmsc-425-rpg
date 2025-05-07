using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Detection : MonoBehaviour
{
    SphereCollider detector;
    public Entity enemyEntityPrefab;
    GameObject levelChanger;
    public int bossLevel = -1;
    [HideInInspector] public PlayerManager playerManager;

    void Start()
    {   
        playerManager = GameObject.FindGameObjectWithTag("PlayerState")?.GetComponent<PlayerManager>();
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger");

        detector = gameObject.AddComponent<SphereCollider>();
        detector.isTrigger = true;
        detector.radius = 1;


    }

    void OnTriggerEnter(Collider other)
    {        
        if (bossLevel != -1) {
            playerManager.currentLevel = bossLevel;
        }
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

        playerManager.inCombat = true;
        playerManager.enemyBeforeCombat = enemyEntityPrefab.enemyId;
        playerManager.enemyPositionBeforeCombat = transform.position;

        // Fades scene to black before changing to combat scene
        FadeTransition transition = levelChanger.GetComponent<FadeTransition>();
        transition.animator = levelChanger.GetComponent<Animator>();
        yield return StartCoroutine(transition.PlayEncounterTransition());

        SceneManager.LoadScene("Scenes/CombatManagerScene");
    }   
}
