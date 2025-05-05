using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoManager : MonoBehaviour
{
    public TMP_Text lvl, xp, sp;
    public Image xpBar;
    Entity playerEntity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xpBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerEntity == null) {
            playerEntity = PlayerManager.player.entity();
            lvl.text = "Level: " + playerEntity.stats.level;
            xp.text = "XP: " + playerEntity.stats.experience + "/" + playerEntity.stats.expToNext;
            sp.text = "Available Points: " + playerEntity.skillPoints;
            xpBar.fillAmount = playerEntity.stats.experience/playerEntity.stats.expToNext;
        }
    }
}
