using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerManager : MonoBehaviour
{
    public Entity player;
    private Stat playerStats;
    public Entity enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = new Entity();
        playerStats = player.stats;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(!enemy.IsAlive()) {
                Debug.Log("enemy dead");
                player.kill(enemy);
                Debug.Log("Gained "+enemy.calculateXPDropped()+" XP!");
                Debug.Log("Player level and xp are now: " + playerStats.level + ", " + playerStats.experience +"/"+playerStats.expToNext);
            } else {
                player.attackEntity(enemy, 100);
                Debug.Log("Dealt "+100+" dmg to enemy. Enemy HP: " + enemy.stats.health);
            }
        }

        
    }
}
