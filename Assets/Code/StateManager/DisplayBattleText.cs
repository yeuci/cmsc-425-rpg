using TMPro;
using UnityEngine;

public class DisplayBattleText : MonoBehaviour
{

    public GameObject DisplayPopup(Transform container, GameObject prefab, string text) {
        GameObject currInstance = BattleManager.instance.currentBattleText;

        if (currInstance != null) {
            Destroy(currInstance);
        }
        
        currInstance = Instantiate(prefab, container);
        currInstance.GetComponentInChildren<TextMeshProUGUI>().text = text;

        return currInstance;
    }
}
