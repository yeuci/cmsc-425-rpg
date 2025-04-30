using UnityEngine;

public class PlaySpellAnimation : MonoBehaviour
{
    public GameObject spellPrefab;
    public Transform enemyPostion;

    public void StartAnimation() {
        GameObject spell = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);
    }

}
