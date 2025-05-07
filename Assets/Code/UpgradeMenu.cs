using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public TMP_Text spAmt;
    public GameObject upgradeWindow, upgradeAvail;
    public GameObject inventoryGameObject;
    int skillPoints;
    bool inventoryOpen;
    void Awake()
    {
        skillPoints = -1;
        inventoryOpen = false;
        if(PlayerManager.player != null) skillPoints = PlayerManager.player.entity().skillPoints;
        if(skillPoints > 0) {
            upgradeAvail.SetActive(true);
            spAmt.text = skillPoints.ToString();      
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeWindow.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {    
        skillPoints = PlayerManager.player.entity().skillPoints;
        
        if(skillPoints == 0) {
            upgradeAvail.SetActive(false);
            spAmt.text = "-";
            skillPoints = -1;
        }
        else if(skillPoints > 0) {
            upgradeAvail.SetActive(true);
            spAmt.text = PlayerManager.player.entity().skillPoints.ToString();
        }

        if(Input.GetKeyDown(KeyCode.U)) {
            upgradeWindow.SetActive(!upgradeWindow.activeSelf);
        }
        
        if(!inventoryOpen && inventoryGameObject.activeSelf) {
            inventoryOpen = true;
        }
        if(inventoryOpen && !inventoryGameObject.activeSelf) {
            upgradeWindow.SetActive(false);
            inventoryOpen = false;
        }
    }
}
