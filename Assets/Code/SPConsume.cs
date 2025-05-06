using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SPConsume : MonoBehaviour
{
    int[] allocated;
    public int startSP, currentSP;
    public Entity playerEntity;
    public TMP_Text available;
    public Button applyButton;

    public bool applied;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(PlayerManager.player != null) {
            playerEntity = PlayerManager.player.entity();
            startSP = currentSP = playerEntity.skillPoints;
        }
    }
    void Start()
    {
        allocated = new int[5];
        startSP = -1;
        currentSP = -1;
        playerEntity = PlayerManager.player.entity();
        startSP = currentSP = playerEntity.skillPoints;
    }

    void Update()
    {
        if(playerEntity == null || startSP != playerEntity.skillPoints) {
            playerEntity = PlayerManager.player.entity();
            startSP = currentSP = playerEntity.skillPoints;
        }

        if(currentSP == startSP) applyButton.interactable = false;
        else applyButton.interactable = true;
    }

    public void Allocate(StatAllocator statAllocator) {
        allocated[(int)statAllocator.statToUpgrade]++;
        currentSP--;
        available.text = "Available Points: " + currentSP;
    }
    public void Deallocate(StatAllocator statAllocator) {
        allocated[(int)statAllocator.statToUpgrade]--;
        currentSP++;
        available.text = "Available Points: " + currentSP;
    }

    public void ApplyUpgrade() {
        PlayerManager.player.entity().applyUpgrade(allocated);
        startSP = currentSP;
        playerEntity.skillPoints = currentSP;
        applyButton.interactable = false;
        applied = true;
        allocated = new int[5];
    }
}
